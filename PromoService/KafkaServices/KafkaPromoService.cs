using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using EntertaimentService.Kafka.Utils;
using Newtonsoft.Json;
using PromoService.Models.PromoApplication.Requests;
using PromoService.Services.PromoApplication;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;

namespace PromoService.KafkaServices
{
    public class KafkaPromoService : KafkaService
    {
        private readonly string _promoResponseTopic = Environment.GetEnvironmentVariable("PROMO_APPLICATION_RESPONSE_TOPIC") ?? "promo-application-response-topic";
        private readonly string _promoRequestTopic = Environment.GetEnvironmentVariable("PROMO_APPLICATION_REQUEST_TOPIC") ?? "promo-application-request-topic";
        private readonly IPromoApplicationService _promoApplicationService;
        public KafkaPromoService(
            ILogger<KafkaService> logger,
            IPromoApplicationService promoApplicationService,
            IProducer<string,string> producer,
            KafkaTopicManager kafkaTopicManager) : base(logger, producer, kafkaTopicManager)
        {
            _promoApplicationService = promoApplicationService;
            base.ConfigureConsumer(_promoRequestTopic);
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
                            case "getMyPromo":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<GetMyPromoApplicationsRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promoResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promoApplicationService.GetMyPromoApplications(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getMyPromo")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    throw new MyKafkaException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promoResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getMyPromo")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("promoService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "registerPromo":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<RegisterPromoUseRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promoResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promoApplicationService.RegisterPromoUse(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("registerPromo")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    throw new MyKafkaException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promoResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("registerPromo")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("promoService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "validatePromo":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<ValidatePromocodeApplicationRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promoResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promoApplicationService.ValidatePromocodeApplication(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("validatePromo")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    throw new MyKafkaException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promoResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("validatePromo")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("promoService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;           
                            default: 
                                _consumer.Commit(consumeResult);
                                
                                throw new ConsumerRecievedMessageInvalidException("Invalid message received");
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