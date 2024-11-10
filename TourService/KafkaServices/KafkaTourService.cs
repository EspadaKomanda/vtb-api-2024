using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Database.Models;
using TourService.Kafka;
using TourService.Kafka.Utils;
using TourService.KafkaException;
using TourService.KafkaException.ConsumerException;
using TourService.Models.Tour.Requests;
using TourService.Models.Tour.Responses;
using TourService.Services.PhotoService;
using TourService.Services.TourService;

namespace TourService.KafkaServices
{
    public class KafkaTourService : KafkaService 
    {
        private readonly string _tourRequestTopic = Environment.GetEnvironmentVariable("TOUR_REQUEST_TOPIC") ?? "tourRequestTopic";
        private readonly string _tourResponseTopic = Environment.GetEnvironmentVariable("TOUR_RESPONSE_TOPIC") ?? "tourResponseTopic"; 
        private readonly ITourService _tourService;
        private readonly IPhotoService _photoService;
        public KafkaTourService(ILogger<KafkaService> logger, IProducer<string, string> producer, KafkaTopicManager kafkaTopicManager, ITourService tourService, IPhotoService photoService) : base(logger, producer, kafkaTopicManager)
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
                            case "addTour":
                                try
                                {
                                    var request = JsonConvert.DeserializeObject<AddTourRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(request))
                                    {
                                        long tourId =  await _tourService.AddTour(
                                                    new Tour()
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
                                                    TourId = tourId
                                                });
                                                counter++;
                                            }
                                        }
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(new AddTourResponse() 
                                            {
                                                TourId =tourId
                                            }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("addTour")),
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
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("addTour")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }

                                break;
                            case "getTour":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetTourRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetTourResponse()
                                                {
                                                    Tour = await _tourService.GetTour(result)
                                                }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getTour")),
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
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getTour")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "getTours":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<GetToursRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    var tours =  _tourService.GetTours(result);
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new GetToursResponse(){ 
                                                    
                                                    Tours =  tours.ToList(),
                                                    Amount = tours.Count(),
                                                    Page = result.Page

                                                }),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("getTours")),
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
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("getTours")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "updateTour":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UpdateTourRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new UpdateTourResponse(){
                                                    IsSuccess = _tourService.UpdateTour(result)
                                                }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("updateTour")),
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
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("updateTour")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "removeTour":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<RemoveTourRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                new RemoveTourResponse(){
                                                    IsSuccess = _tourService.RemoveTour(result)
                                                }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("removeTour")),
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
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("removeTour")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "linkTags":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<LinkTagsRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                 new LinkTagsResponse(){
                                                    IsSuccess = await _tourService.LinkTags(result)
                                                 }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("linkTags")),
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
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("linkTags")), 
                                            new Header("sender", Encoding.UTF8.GetBytes("tourService")), 
                                            new Header("error", Encoding.UTF8.GetBytes(e.Message))
                                        ]
                                    });
                                    _consumer.Commit(consumeResult);
                                    _logger.LogError(e, "Error sending message");
                                }
                                break;
                            case "unLinkTag":
                                try
                                {
                                    var result = JsonConvert.DeserializeObject<UnlinkTagRequest>(consumeResult.Message.Value) ?? throw new NullReferenceException("result is null");
                                    if(base.IsValid(result))
                                    {
                                        if(await base.Produce(_tourResponseTopic,new Message<string, string>()
                                        {
                                            Key = consumeResult.Message.Key,
                                            Value = JsonConvert.SerializeObject(
                                                 new UnlinkResponse(){
                                                    IsSuccess =  _tourService.UnlinkTag(result)
                                                 }
                                                ),
                                            Headers = [
                                                new Header("method",Encoding.UTF8.GetBytes("unLinkTags")),
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
                                     _ = await base.Produce(_tourResponseTopic, new Message<string, string>()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Value = JsonConvert.SerializeObject(new MessageResponse(){ Message = e.Message}),
                                        Headers = [
                                            new Header("method", Encoding.UTF8.GetBytes("unLinkTags")), 
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