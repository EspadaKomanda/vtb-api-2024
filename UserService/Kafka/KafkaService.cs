using System.ComponentModel;
using System.Text;
using Confluent.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using Newtonsoft.Json;
namespace TourService.Kafka;

public abstract class KafkaService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager)
{
    protected readonly IProducer<string, string> _producer = producer;
    protected readonly ILogger<KafkaService> _logger = logger;
    protected readonly KafkaTopicManager _kafkaTopicManager = kafkaTopicManager;
    protected IConsumer<string, string>?_consumer;

    protected void ConfigureConsumer(string topicName)
    {
        try
        {
            var config = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            if(IsTopicAvailable(topicName))
            {
                _consumer.Subscribe(topicName);
            }
            throw new ConsumerTopicUnavailableException("Topic unavailable");
        }
        catch (Exception e)
        {
            if (e is MyKafkaException)
            {
                _logger.LogError(e,"Error configuring consumer");
                throw new ConsumerException("Error configuring consumer",e);
            }
            _logger.LogError(e,"Unhandled error");
            throw;
        }
    }
    private bool IsTopicAvailable(string topicName)
    {
        try
        {
             bool IsTopicExists = _kafkaTopicManager.CheckTopicExists(topicName);
                if (IsTopicExists)
                {
                    return IsTopicExists;
                }
                _logger.LogError("Unable to subscribe to topic");
                throw new ConsumerTopicUnavailableException("Topic unavailable");
           
        }
        catch (Exception e)
        {
            if (e is MyKafkaException)
            {
                _logger.LogError(e,"Error checking topic");
                throw new ConsumerException("Error checking topic",e);
            }
            _logger.LogError(e,"Unhandled error");
            throw;
        }
    }
    public abstract Task Consume();
    public async Task<bool> Produce( string topicName,Message<string, string> message)
    {
        try
        {
            bool IsTopicExists = IsTopicAvailable(topicName);
            if (IsTopicExists)
            {
                var deliveryResult = await _producer.ProduceAsync(topicName, message);
                if (deliveryResult.Status == PersistenceStatus.Persisted)
                {
    
                    _logger.LogInformation("Message delivery status: Persisted {Result}", deliveryResult.Value);
                    return true;
                }
                
                _logger.LogError("Message delivery status: Not persisted {Result}", deliveryResult.Value);
                throw new MessageProduceException("Message delivery status: Not persisted" + deliveryResult.Value);
                
            }
            
            bool IsTopicCreated = _kafkaTopicManager.CreateTopic(topicName, Convert.ToInt32(Environment.GetEnvironmentVariable("PARTITIONS_STANDART")), Convert.ToInt16(Environment.GetEnvironmentVariable("REPLICATION_FACTOR_STANDART")));
            if (IsTopicCreated)
            {
                var deliveryResult = await _producer.ProduceAsync(topicName, message);
                if (deliveryResult.Status == PersistenceStatus.Persisted)
                {
                    _logger.LogInformation("Message delivery status: Persisted {Result}", deliveryResult.Value);
                    return true;
                }
                
                _logger.LogError("Message delivery status: Not persisted {Result}", deliveryResult.Value);
                throw new MessageProduceException("Message delivery status: Not persisted");
                
            }
            _logger.LogError("Topic unavailable");
            throw new MessageProduceException("Topic unavailable");
        }
        catch (Exception e)
        {
            if (e is MyKafkaException)
            {
                _logger.LogError(e, "Error producing message");
                throw new ProducerException("Error producing message",e);
            }
            throw;
        }
       
       
        
    }

    
}