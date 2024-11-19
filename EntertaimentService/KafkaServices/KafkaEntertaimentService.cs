using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using EntertaimentService.Database.Models;
using EntertaimentService.Kafka;
using EntertaimentService.Kafka.Utils;
using EntertaimentService.KafkaException;
using EntertaimentService.KafkaException.ConsumerException;
using EntertaimentService.Models.Entertaiment.Requests;
using EntertaimentService.Services.PhotoService;
using EntertaimentService.Services.EntertaimentServices;
using EntertaimentService.Models.Tour.Responses;
using EntertaimentService.Models.Tour.Requests;
using Entertainments.Models.Tour.Responses;
using TourService.Kafka;

namespace EntertaimentService.KafkaServices
{
    public class KafkaEntertaimentService: KafkaService 
    {

        //FIXME: If mykafkaException i have to reconfigure the producer
        private readonly string _tourRequestTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_REQUEST_TOPIC") ?? "entertaimentRequestTopic";
        private readonly string _tourResponseTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_RESPONSE_TOPIC") ?? "entertaimentResponseTopic"; 
        private readonly IEntertaimentService _tourService;
        private readonly IPhotoService _photoService;
        public KafkaEntertaimentService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, IEntertaimentService tourService, IPhotoService photoService) : base(logger, producer, kafkaTopicManager)
        {
            _tourService = tourService;
            _photoService = photoService;
            base.ConfigureConsumer(_tourRequestTopic);
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
                            case "addEntertaiment":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<AddEntertaimentRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        long tourId =  await _tourService.AddEntertaiment(
                                                    new Entertaiment()
                                                    {
                                                        Name = request.Name,
                                                        Description = request.Description,
                                                        Price = request.Price,
                                                        Address = request.Address,
                                                        Comment = request.Comment,
                                                        Coordinates = request.Coordinates,
                                                        IsActive = request.IsActive

                                                    });
                                        if(request.PhotoBytes != null)
                                        {
                                            
                                            int counter = 1;
                                            foreach(var photo in request.PhotoBytes)
                                            {
                                                await _photoService.AddPhoto(new Models.Photos.Requests.AddPhotoRequest()
                                                {
                                                    PhotoBytes = photo,
                                                    PhotoName = request.Name+counter,
                                                    EntertaimentId = tourId
                                                });
                                                counter++;
                                            }
                                        }
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(new AddEntertaimentResponse() 
                                            {
                                                EntertaimentId =tourId
                                            }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("addEntertaiment")),
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
                                    
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("addEntertaiment")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "getEntertaiment":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetEntertaimentRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetEntertaimentResponse()
                                                {
                                                    Entertaiment = await _tourService.GetEntertaiment(result)
                                                }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getEntertaiment")),
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
                                   
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getEntertaiment")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "getEntertaiments":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetEntertaimentsRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    var tours =  _tourService.GetEntertaiments(result);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetEntertaimentsResponse(){ 
                                                    
                                                    Entertaiments =  tours.ToList(),
                                                    Amount = tours.Count(),
                                                    Page = result.Page

                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getEntertaiments")),
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
                                    
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getEntertaiments")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "updateEntertaiment":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UpdateEntertaimentRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new UpdateEntertaimentResponse(){
                                                    IsSuccess = _tourService.UpdateEntertaiment(result)
                                                }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("updateEntertaiment")),
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
                                   
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("updateEntertaiment")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "removeEntertaiment":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<RemoveEntertaimentRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new RemoveEntertaimentResponse(){
                                                    IsSuccess = _tourService.RemoveEntertaiment(result)
                                                }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("removeEntertaiment")),
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
                                    
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("removeEntertaiment")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                        
                            case "linkCategories":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<LinkCategoriesRequests>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                 new LinkCategoriesResponse(){
                                                    IsSuccess = await _tourService.LinkCategories(result)
                                                 }
                                                ),
                                            Headers = [
                                                 new Header("method", Encoding.UTF8.GetBytes("linkCategories")), 
                                                new Header("sender", Encoding.UTF8.GetBytes("tourService"))
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
                                    
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("linkCategories")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "unlinkCategory":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UnlinkCategoryRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                 new UnlinkResponse(){
                                                    IsSuccess =  _tourService.UnlinkCategory(result)
                                                 }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("unlinkCategory")),
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
                                    
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("unlinkCategory")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "linkPayments":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<LinkPaymentMethodsRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                 new LinkPaymentsResponse(){
                                                    IsSuccess = await _tourService.LinkPaymentMethods(result)
                                                 }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("linkPayments")),
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
                                    
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("linkPayments")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "unlinkPayment":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UnlinkPaymentMethodRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                 new UnlinkResponse(){
                                                    IsSuccess =  _tourService.UnlinkPaymentMethod(result)
                                                 }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("unlinkPayment")),
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
                                   
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("unlinkPayment")), 
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