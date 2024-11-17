using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.PaymentMethod.Requests;
using ApiGatewayService.Models.TourService.Models.PaymentMethod.Responses;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;

namespace ApiGatewayService.Services.TourService.PaymentMethods
{
    public class TourPaymentMethodsService(ILogger<TourPaymentMethodsService> logger, KafkaRequestService kafkaRequestService) : ITourPaymentMethodsService
    {
        private readonly ILogger<TourPaymentMethodsService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("TOUR_PAYMENT_METHODS_REQUEST_TOPIC");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("TOUR_PAYMENT_METHODS_RESPONSE_TOPIC");
        public async Task<AddPaymendMethodResponse> AddPaymentMethod(AddPaymentMethodRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("addPaymentMethod")),
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
                    return _kafkaRequestService.GetMessage<AddPaymendMethodResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<UpdatePaymentMethodResponse> UpdatePaymentMethod(UpdatePaymentMethodRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("updatePaymentMethod")),
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
                    return _kafkaRequestService.GetMessage<UpdatePaymentMethodResponse>(messageId.ToString(),responseTopic); 
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<GetPaymentMethodResponse> GetPaymentMethod(GetPaymentMethodRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("getPaymentMethod")),
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
                    return _kafkaRequestService.GetMessage<GetPaymentMethodResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<RemovePaymentMethodResponse> RemovePaymentMethod(RemovePaymentMethodRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("removePaymentMethod")),
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
                    return _kafkaRequestService.GetMessage<RemovePaymentMethodResponse>(messageId.ToString(),responseTopic);
                
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}