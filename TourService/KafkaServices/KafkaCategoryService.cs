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
using TourService.Models.Benefits;
using TourService.Models.Category.Requests;
using TourService.Models.Category.Responses;
using TourService.Services.CategoryService;

namespace TourService.KafkaServices
{
    public class KafkaCegoryService : KafkaService
    {
        private readonly string _categoryRequestTopic = Environment.GetEnvironmentVariable("CATEGORY_REQUEST_TOPIC") ?? "categoryRequestTopic";
        private readonly string _categoryResponseTopic = Environment.GetEnvironmentVariable("CATEGORY_RESPONSE_TOPIC") ?? "categoryResponseTopic"; 
        private readonly ICategoryService _categoryService;

        public KafkaCegoryService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, ICategoryService categoryService) : base(logger, producer, kafkaTopicManager)
        {
            _categoryService = categoryService;
            base.ConfigureConsumer(_categoryRequestTopic);
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
                            case "addCategory":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<AddCategoryRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        if(await base.Produce(_categoryResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(new AddCategoryResponse() 
                                            {
                                                CategoryId = await _categoryService.AddCategory(new Database.Models.Category()
                                                {
                                                    Name = request.CategoryName
                                                })
                                            }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("addCategory")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_categoryResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("addCategory")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "getCategory":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetCategoryRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_categoryResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetCategoryResponse(){ 
                                                    Category = await _categoryService.GetCategory(result)
                                                    }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getCategory")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_categoryResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getCategory")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "getCategories":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetCategoriesRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_categoryResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetCategoriesResponse(){ 
                                                    Categories = _categoryService.GetCategories(result).ToList()
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getCategories")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_categoryResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getCategories")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "removeCategory":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<RemoveCategoryRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_categoryRequestTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new RemoveCategoryResponse(){ 
                                                    IsSuccess = _categoryService.RemoveCategory(result)
                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("removeCategory")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_categoryResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("removeCategory")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "updateCategory":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UpdateCategoryRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_categoryResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new UpdateCategoryResponse(){
                                                     IsSuccess =_categoryService.UpdateCategory(result)
                                                    }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("updateCategory")),
                                                new Header("sender",Encoding.UTF8.GetBytes("tourService"))
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
                                    _ = await base.Produce(_categoryResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("updateCategory")),
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