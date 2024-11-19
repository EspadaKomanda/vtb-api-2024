using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.Models.Tour.Requests;
using TourService.Models.Tour.Responses;

namespace ApiGatewayService.Services.TourService.Tours
{
    public class TourService(ILogger<TourService> logger, KafkaRequestService kafkaRequestService) : ITourService
    {
        private readonly ILogger<TourService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("TOUR_REQUEST_TOPIC");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("TOUR_RESPONSE_TOPIC");
        
        public async Task<AddTourResponse> AddTour(AddTourRequest addTourRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(addTourRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("addTour")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<AddTourResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in AddTour");
                throw;
            }
        }
        public async Task<GetTourResponse> GetTour(GetTourRequest getTourRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(getTourRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getTour")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<GetTourResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in GetTour");
                throw;
            }
        }
        public async Task<UpdateTourResponse> UpdateTour(UpdateTourRequest updateTourRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(updateTourRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("updateTour")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UpdateTourResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in UpdateTour");
                throw;
            }
        }
        public async Task<RemoveTourResponse> RemoveTour(RemoveTourRequest removeTourRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(removeTourRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("removeTour")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<RemoveTourResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in RemoveTour");
                throw;
            }
        }
        public async Task<GetToursResponse> GetTours(GetToursRequest getToursRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(getToursRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getTours")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<GetToursResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in GetTours");
                throw;
            }
        }
        public async Task<LinkCategoriesResponse> LinkCategories(LinkCategoriesRequests linkCategoriesRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(linkCategoriesRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("linkCategories")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<LinkCategoriesResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in LinkCategories");
                throw;
            }
        }
        public async Task<UnlinkResponse> UnlinkCategory(UnlinkCategoryRequest unlinkCategoryRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(unlinkCategoryRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("unlinkCategory")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UnlinkResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in UnlinkCategory");
                throw;
            }
        }
        public async Task<LinkPaymentsResponse> LinkPaymentMethods(LinkPaymentMethodsRequest linkPaymentMethodsRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(linkPaymentMethodsRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("linkPayments")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<LinkPaymentsResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in LinkPaymentMethods");
                throw;
            }
        }
        public async Task<UnlinkResponse> UnlinkPaymentMethod(UnlinkPaymentMethodRequest unlinkPaymentMethodRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(unlinkPaymentMethodRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("unlinkPaymentMethod")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UnlinkResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in UnlinkPaymentMethod");
                throw;
            }
        }
        public async Task<LinkTagsResponse> LinkTags(LinkTagsRequest linkTagsRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(linkTagsRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("linkTags")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<LinkTagsResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in LinkTags");
                throw;
            }
        }
        public async Task<UnlinkResponse> UnlinkTag(UnlinkTagRequest unlinkTagRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(unlinkTagRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("unlinkTag")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(requestTopic,message,responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UnlinkResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in UnlinkTag");
                throw;
            }
        }
    }
}