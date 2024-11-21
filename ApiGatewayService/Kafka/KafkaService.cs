using System.ComponentModel;
using System.Text;
using Confluent.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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
                GroupId = "apigateway-service-consumer-group",
                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKERS"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            if(IsTopicAvailable(topicName))
            {
                var partitions = new List<TopicPartition>();
                partitions.Add(new TopicPartition(topicName, 0));

                _consumer.Assign(partitions);
                return;
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
            else
            {
                return _kafkaTopicManager.CreateTopic(topicName, 3, 1);
            }
           
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
                var deliveryResult = await _producer.ProduceAsync(
                    new TopicPartition(topicName, new Partition(0));
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
                var deliveryResult = await _producer.ProduceAsync(
                    new TopicPartition(topicName, new Partition(0));
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
    protected bool IsValid(object value)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(value, null, null);
        
        bool isValid = Validator.TryValidateObject(value, validationContext, validationResults, true);

        if (!isValid)
        {
            foreach (var validationResult in validationResults)
            {
                _logger.LogError(validationResult.ErrorMessage);
            }
        }

        return isValid;
    }
    
}