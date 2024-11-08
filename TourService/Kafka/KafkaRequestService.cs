using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using TourService.Kafka.Utils;
using TourService.KafkaException.ConfigurationException;

namespace TourService.Kafka
{
    public class KafkaRequestService
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaRequestService> _logger;
        private readonly KafkaTopicManager _kafkaTopicManager;
        private readonly HashSet<PendingMessagesBus> _pendingMessagesBus;
        private readonly HashSet<RecievedMessagesBus> _recievedMessagesBus;
        private readonly HashSet<IConsumer<string,string>> _consumerPool;
        public KafkaRequestService(
            IProducer<string, string> producer,
            ILogger<KafkaRequestService> logger,
            KafkaTopicManager kafkaTopicManager,
            List<string> responseTopics,
            List<string> requestsTopics)
        {
            _producer = producer;
            _logger = logger;
            _kafkaTopicManager = kafkaTopicManager;
            _recievedMessagesBus = ConfigureRecievedMessages(responseTopics);
            _pendingMessagesBus = ConfigurePendingMessages(requestsTopics);
            _consumerPool = ConfigureConsumers(responseTopics.Count());
            
        }
        public void BeginRecieving(List<string> responseTopics)
        {
            int topicCount = 0;
            foreach(var consumer in _consumerPool)
            {

                Thread thread = new Thread(async x=>{
                    await Consume(consumer,responseTopics[topicCount]);
                });
                thread.Start();
                topicCount++;
            }
        }
       
        private HashSet<IConsumer<string,string>> ConfigureConsumers(int amount)
        {
            try
            {
                if(amount<=0)
                {
                    throw new ConfigureConsumersException(" Amount of consumers must be above 0!");
                }
                HashSet<IConsumer<string,string>> consumers = new HashSet<IConsumer<string, string>>();
                for (int i = 0; i < amount; i++)
                {
                    consumers.Add(
                        new ConsumerBuilder<string,string>(
                            new ConsumerConfig()
                            {
                                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKERS"),
                                GroupId = "gatewayConsumer"+Guid.NewGuid().ToString(), 
                                EnableAutoCommit = true,
                                AutoCommitIntervalMs = 10,
                                EnableAutoOffsetStore = true,
                                AutoOffsetReset = AutoOffsetReset.Earliest

                            }
                        ).Build()
                    );
                }
                return consumers;
            }
            catch (Exception ex)
            {
                if (ex is MyKafkaException)
                {
                    _logger.LogError(ex, "Error configuring consumers");
                    throw new ProducerException("Error configuring consumers",ex);
                }
                throw;
            }
          
        }
        private HashSet<PendingMessagesBus> ConfigurePendingMessages(List<string> ResponseTopics)
        {
            if(ResponseTopics.Count == 0)
            {
                throw new ConfigureMessageBusException("At least one requests topic must e provided!");
            }
            var PendingMessages = new HashSet<PendingMessagesBus>();
            foreach(var requestTopic in ResponseTopics)
            {
                PendingMessages.Add(new PendingMessagesBus(){ TopicName=requestTopic, MessageKeys = new HashSet<Utils.MethodKeyPair>()});
            }
            return PendingMessages;
        }
        private HashSet<RecievedMessagesBus> ConfigureRecievedMessages(List<string> ResponseTopics)
        {
            if(ResponseTopics.Count == 0)
            {
                throw new ConfigureMessageBusException("At least one response topic must e provided!");
            }
            HashSet<RecievedMessagesBus> Responses = new HashSet<RecievedMessagesBus>();
            foreach(var RequestTopic in ResponseTopics)
            {
                Responses.Add(new RecievedMessagesBus() { TopicName = RequestTopic, Messages = new HashSet<Message<string, string>>()});
            }
            return Responses;
        }

        private bool IsTopicAvailable(string topicName)
        {
            try
            {
                if(_kafkaTopicManager.CheckTopicExists(topicName))
                {
                    return true;
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

        public bool IsMessageRecieved(string MessageKey)
        {
            try
            {
                return _recievedMessagesBus.Any(x=>x.Messages.Any(x=>x.Key==MessageKey));
            }
            catch (Exception e)
            {
                throw new ConsumerException($"Recieved message bus error",e);
            }
        }
        public async Task<bool> Produce(string topicName, Message<string, string> message, string responseTopic)
        {
            try
            {
                bool IsTopicExists = IsTopicAvailable(topicName);
                if (IsTopicExists && IsTopicPendingMessageBusExist( responseTopic))
                {
                    var deliveryResult = await _producer.ProduceAsync(topicName, message);
                    if (deliveryResult.Status == PersistenceStatus.Persisted)
                    {
                        _logger.LogInformation("Message delivery status: Persisted {Result}", deliveryResult.Value);
                      
                            _pendingMessagesBus.FirstOrDefault(x=>x.TopicName == responseTopic).MessageKeys.Add(new MethodKeyPair(){
                            MessageKey = message.Key,
                            MessageMethod = Encoding.UTF8.GetString(message.Headers.FirstOrDefault(x => x.Key.Equals("method")).GetValueBytes())
                        });
                        return true;
                        
                        
                    }
                    
                    _logger.LogError("Message delivery status: Not persisted {Result}", deliveryResult.Value);
                    throw new MessageProduceException("Message delivery status: Not persisted" + deliveryResult.Value);
                    
                }
                
                bool IsTopicCreated = _kafkaTopicManager.CreateTopic(topicName, Convert.ToInt32(Environment.GetEnvironmentVariable("PARTITIONS_STANDART")), Convert.ToInt16(Environment.GetEnvironmentVariable("REPLICATION_FACTOR_STANDART")));
                if (IsTopicCreated && IsTopicPendingMessageBusExist( responseTopic))
                {
                    var deliveryResult = await _producer.ProduceAsync(topicName, message);
                    if (deliveryResult.Status == PersistenceStatus.Persisted)
                    {
                        _logger.LogInformation("Message delivery status: Persisted {Result}", deliveryResult.Value);
                        _pendingMessagesBus.FirstOrDefault(x=>x.TopicName == responseTopic).MessageKeys.Add(new MethodKeyPair(){
                            MessageKey = message.Key,
                            MessageMethod = Encoding.UTF8.GetString(message.Headers.FirstOrDefault(x => x.Key.Equals("method")).GetValueBytes())
                        });
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
        private bool IsTopicPendingMessageBusExist(string responseTopic)
        {
            return _pendingMessagesBus.Any(x => x.TopicName == responseTopic);
        }
        private async Task Consume(IConsumer<string,string> localConsumer,string topicName)
        {   
            localConsumer.Subscribe(topicName);
            while (true)
            {
                ConsumeResult<string, string> result = localConsumer.Consume();

                if (result != null)
                {
                    try
                    {
                        if( _pendingMessagesBus.FirstOrDefault(x=>x.TopicName==topicName).MessageKeys.Any(x=>x.MessageKey==result.Message.Key))
                        {
                            if(result.Message.Headers.Any(x => x.Key.Equals("errors")))
                            {
                                var errors = Encoding.UTF8.GetString(result.Message.Headers.FirstOrDefault(x => x.Key.Equals("errors")).GetValueBytes());
                                _logger.LogError(errors);
                                
                                throw new ConsumerException(errors);
                            }
                                
                            MethodKeyPair pendingMessage = _pendingMessagesBus.FirstOrDefault(x=>x.TopicName==topicName).MessageKeys.FirstOrDefault(x=>x.MessageKey==result.Message.Key);
                            if(_pendingMessagesBus.FirstOrDefault(x=>x.TopicName==topicName).MessageKeys.Any(x=>x.MessageMethod== Encoding.UTF8.GetString(result.Message.Headers.FirstOrDefault(x => x.Key.Equals("method")).GetValueBytes())))
                            {
                              
                                localConsumer.Commit(result);
                                _recievedMessagesBus.FirstOrDefault(x=>x.TopicName== topicName).Messages.Add(result.Message);
                                _pendingMessagesBus.FirstOrDefault(x=>x.TopicName==topicName).MessageKeys.Remove(pendingMessage);
                            }
                            _logger.LogError("Wrong message method");
                            throw new ConsumerException("Wrong message method");
                        }   
                    }
                    catch (Exception e)
                    {
                        if (e is MyKafkaException)
                        {
                            _logger.LogError(e,"Consumer error");
                            throw new ConsumerException("Consumer error ",e);
                        }
                        _logger.LogError(e,"Unhandled error");
                        localConsumer.Commit(result);
                        throw;
                    }
                   
                }
            }
        }
    }
}