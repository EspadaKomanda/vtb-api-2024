using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using EntertaimentService.Kafka.Utils;
using Newtonsoft.Json;
using PromoService.Models.Promocode.Requests;
using PromoService.Services.Promocode;
using TourService.Kafka;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;

namespace PromoService.KafkaServices
{
    public class KafkaPromocodeService : KafkaService
    {
        private readonly string _promocodeResponseTopic = Environment.GetEnvironmentVariable("PROMOCODE_RESPONSE_TOPIC") ?? "promocode-response-topic";
        private readonly string _promocodeRequestTopic = Environment.GetEnvironmentVariable("PROMOCODE_REQUEST_TOPIC") ?? "promocode-request-topic";
        private readonly IPromocodeService _promocodeService;
        public KafkaPromocodeService(
            ILogger<KafkaService> logger,
            IPromocodeService promocodeService,
            IProducer<string,string> producer,
            KafkaTopicManager kafkaTopicManager) : base(logger, producer, kafkaTopicManager)
        {
            _promocodeService = promocodeService;
            base.ConfigureConsumer(_promocodeRequestTopic);
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
                            case "createPromocode":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<CreatePromocodeRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promocodeResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promocodeService.CreatePromocode(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("createPromocode")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promocodeResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("createPromocode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("promoService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "deletePromocode":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<DeletePromocodeRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promocodeResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promocodeService.DeletePromocode(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("deletePromocode")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promocodeResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("deletePromocode")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("promoService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "getActivePromocodes":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<GetActivePromocodesRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promocodeResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promocodeService.GetActivePromocodes(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getActivePromocodes")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promocodeResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getActivePromocodes")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("promoService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break; 
                            case "getPromocodes":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<GetPromocodesRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promocodeResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promocodeService.GetPromocodes(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getPromocodes")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promocodeResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getPromocodes")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("promoService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }  
                                break;
                            case "setPromocodeMeta":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<SetPromocodeMetaRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_promocodeResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject( _promocodeService.SetPromocodeMeta(request)),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("setPromocodeMeta")),
                                                new Header("sender",Encoding.UTF8.GetBytes("promoService")),
                                            ]
                                        }))
                                        {

                                            _logger.LogDebug("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                            break;
                                        }
                                    }
                                    _logger.LogError("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    _ = await base.Produce(_promocodeResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("setPromocodeMeta")), 
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