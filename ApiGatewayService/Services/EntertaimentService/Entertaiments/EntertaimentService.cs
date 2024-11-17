using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiGatewayService.Services.EntertaimentService.Categories;
using Confluent.Kafka;
using EntertaimentService.Models.Entertaiment.Requests;
using EntertaimentService.Models.Tour.Requests;
using EntertaimentService.Models.Tour.Responses;
using EntertainmentService.Models.Tour.Responses;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException.ConsumerException;

namespace ApiGatewayService.Services.EntertaimentService.Entertaiments
{
    public class EntertaimentService(ILogger<EntertaimentService> logger, KafkaRequestService kafkaRequestService) : IEntertaimentService
    {
        private readonly ILogger<EntertaimentService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_REQUEST_TOPIC");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_RESPONSE_TOPIC");

        public async Task<AddEntertaimentResponse> AddEntertaiment(AddEntertaimentRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("addEntertaiment")),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<AddEntertaimentResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<GetEntertaimentResponse> GetEntertaiment(GetEntertaimentRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getEntertaiment")),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<GetEntertaimentResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<GetEntertaimentsResponse> GetEntertaiments(GetEntertaimentsRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getEntertaiments")),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString()); 
                    return _kafkaRequestService.GetMessage<GetEntertaimentsResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<RemoveEntertaimentResponse> RemoveEntertaiment(RemoveEntertaimentRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("removeEntertaiment")),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<RemoveEntertaimentResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<UpdateEntertaimentResponse> UpdateEntertaiment(UpdateEntertaimentRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("updateEntertaiment")),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UpdateEntertaimentResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<LinkCategoriesResponse> LinkCategories(LinkCategoriesEntertaimentRequests request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<LinkCategoriesResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<LinkPaymentsResponse> LinkPaymentMethods(LinkPaymentMethodsEntertaimentRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<LinkPaymentsResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<UnlinkResponse> UnlinkCategory(UnlinkCategoryEntertaimentRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("unlinkCategories")),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UnlinkResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<UnlinkResponse> UnlinkPaymentMethod(UnlinkPaymentMethodEntertaimentRequest request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("unlinkPayments")),
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
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UnlinkResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}