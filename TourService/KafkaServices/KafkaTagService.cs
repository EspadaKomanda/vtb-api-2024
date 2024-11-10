using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.Kafka.Utils;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using TourService.Models.Tag.Requests;
using TourService.Models.Tag.Responses;
using TourService.Services.TagService;

namespace TourService.KafkaServices
{
    public class KafkaTagService : KafkaService
    {
        private readonly string _tagRequestTopic = Environment.GetEnvironmentVariable("TAG_REQUEST_TOPIC") ?? "tagRequestTopic";
        private readonly string _tagResponseTopic = Environment.GetEnvironmentVariable("TAG_RESPONSE_TOPIC") ?? "tagResponseTopic"; 
        private readonly ITagService _tagService;

        public KafkaTagService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, ITagService tagService) : base(logger, producer, kafkaTopicManager)
        {
            _tagService = tagService;
            base.ConfigureConsumer(_tagRequestTopic);
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
                            case "addTag":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<AddTagRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_tagResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(new AddTagResponse() 
                                            {
                                                TagId = await _tagService.AddTag(new Database.Models.Tag()
                                                {
                                                    Name = request.TagName
                                                })
                                            }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("addTag")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                     _ = await base.Produce(_tagResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("addTag")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "getTag":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetTagRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tagResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetTagResponse(){ 
                                                    tag = await _tagService.GetTag(result)
                                                    }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getTag")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                     _ = await base.Produce(_tagResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getTag")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "getTags":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetTagsRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tagResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetTagsResponse(){ 
                                                    Tags = _tagService.GetTags(result).ToList()
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getTags")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                     _ = await base.Produce(_tagResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getTags")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "removeTag":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<RemoveTagRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tagResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new RemoveTagResponse(){ 
                                                    IsSuccess = _tagService.RemoveTag(result)
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("removeTag")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                     _ = await base.Produce(_tagResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("removeTag")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "updateTag":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UpdateTagRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tagResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new UpdateTagResponse(){
                                                     IsSuccess =_tagService.UpdateTag(result)
                                                    }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("updateTag")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                     _ = await base.Produce(_tagResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("updateTag")),
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")),
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