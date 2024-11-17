using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using EntertaimentService.Kafka;
using EntertaimentService.Kafka.Utils;
using EntertaimentService.KafkaException;
using EntertaimentService.KafkaException.ConsumerException;
using EntertaimentService.Models.Wishlist.Requests;
using EntertaimentService.Models.Wishlist.Responses;
using TourService.Services.WishlistService;
using EntertaimentService.Services.WishlistService;
using TourService.Kafka;

namespace TourService.KafkaServices
{
    public class KafkaWishlistService : KafkaService
    {
        private readonly string _wishlistRequestTopic = Environment.GetEnvironmentVariable("WHISHLIST_REQUEST_TOPIC") ?? "wishlistRequestTopic";
        private readonly string _wishlistResponseTopic = Environment.GetEnvironmentVariable("WHISHLIST_RESPONSE_TOPIC") ?? "wishlistResponseTopic"; 
        private readonly IWishlistService _wishlistService;
        public KafkaWishlistService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, IWishlistService wishlistService) : base(logger, producer, kafkaTopicManager)
        {
            _wishlistService = wishlistService;
            base.ConfigureConsumer(_wishlistRequestTopic);
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
                            case "wishlistEntertaiment":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<WishlistEntertaimentRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_wishlistResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(new WishlistEntertaimentResponse() 
                                            {
                                                IsSuccess = await _wishlistService.AddEntertaimentToWishlist(request)
                                            }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("wishlistEntertaiment")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Request validation error");
                                }
                                catch (Exception e)
                                {
                                    if(e is MyKafkaException)
                                    {
                                        _logger.LogError(e,"Error sending message");
                                        throw;
                                    }
                                     _ = await base.Produce(_wishlistResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("wishlistEntertaiment")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "getWishlistedEntertaiments":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetWishlistedEntertaimentsRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_wishlistResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                _wishlistService.GetWishlists(result)
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getWishlistedEntertaiments")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    if(e is MyKafkaException)
                                    {
                                        _logger.LogError(e,"Error sending message");
                                        throw;
                                    }
                                     _ = await base.Produce(_wishlistResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getWishlistedEntertaiments")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "unwishlistEntertaiment":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UnwishlistEntertaimentRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_wishlistResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new UnwishlistEntertaimentResponse(){ 
                                                    IsSuccess = _wishlistService.UnwishlistEntertaiment(result)
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("unwishlistEntertaiment")),
                                                new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                                            ]
                                        }))
                                        {

                                            _logger.LogInformation("Successfully sent message {Key}",consumeResult.Message.Key);
                                            _consumer.Commit(consumeResult);
                                        }
                                    }
                                    _logger.LogError("Request validation error");
                                    throw new RequestValidationException("Invalid request");
                                }
                                catch (Exception e)
                                {
                                    if(e is MyKafkaException)
                                    {
                                        _logger.LogError(e,"Error sending message");
                                        throw;
                                    }
                                     _ = await base.Produce(_wishlistResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("unwishlistEntertaiment")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("entertaimentService")), 
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
                if(_consumer != null)
                { 
                    _consumer.Dispose();
                }
                if (ex is MyKafkaException)
                {
                    _logger.LogError(ex,"Consumer error");
                    throw new ConsumerException("Consumer error ",ex);
                }
                else
                {
                    _logger.LogError(ex,"Unhandled error");
                    throw;
                }
            }
        }
    
        
    }
}