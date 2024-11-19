using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Models.AccessDataCache.Requests;
using AuthService.Services.AccessDataCache;
using AuthService.Services.Models;
using Confluent.Kafka;
using EntertaimentService.Kafka.Utils;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;

namespace AuthService.KafkaServices
{
    public class KafkaAccesDataCacheService : KafkaService
    {
        private readonly string _dataCacheRequestTopic = Environment.GetEnvironmentVariable("DATA_CACHE_REQUEST_TOPIC") ?? "dataCacheRequestTopic";
        private readonly string _dataCacheResponseTopic = Environment.GetEnvironmentVariable("DATA_CACHE_RESPONSE_TOPIC") ?? "dataCacheResponseTopic";
        private readonly IAccessDataCacheService _accessDataCacheService;
        public KafkaAccesDataCacheService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, IAccessDataCacheService dataCacheService) : base(logger, producer, kafkaTopicManager)
        {
            _accessDataCacheService = dataCacheService;
            base.ConfigureConsumer(_dataCacheRequestTopic);
        }
        public override async Task Consume()
        {
            try
            {
               
                while (true)
                {
                    if(_consumer == null)
                    {
                        _logger.LogError("Consumer is null");
                        throw new ConsumerException("Consumer is null");
                    }
                    ConsumeResult<string, string> consumeResult = _consumer.Consume();
                    if (consumeResult != null)
                    {
                        var headerBytes = consumeResult.Message.Headers
                        .FirstOrDefault(x => x.Key.Equals("method")) ?? throw new NullReferenceException("headerBytes is null");
                    
                  
                        var methodString = Encoding.UTF8.GetString(headerBytes.GetValueBytes());
                        switch (methodString)
                        {
                            case "recacheUser":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<RecacheUserRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_dataCacheResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                await _accessDataCacheService.RecacheUser(request)
                                            ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("recacheUser")),
                                                new Header("sender",Encoding.UTF8.GetBytes("authService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_dataCacheResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("recacheUser")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("authService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                    
                                }

                                break;
                            default: 
                                _consumer.Commit(consumeResult);
                                break;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                if (ex is MyKafkaException)
                {
                    _logger.LogError(ex,"Consumer error");
                }
                else
                {
                    _logger.LogError(ex,"Unhandled error");
                }
            }
        }
    
    }
}