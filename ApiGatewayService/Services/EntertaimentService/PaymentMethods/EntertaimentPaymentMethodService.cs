using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using EntertaimentService.Models.PaymentMethod.Requests;
using EntertaimentService.Models.PaymentMethod.Responses;
using EntertaimentService.Models.PaymentVariant.Requests;
using Newtonsoft.Json;
using TourService.Kafka;

namespace ApiGatewayService.Services.EntertaimentService.PaymentMethods
{
    public class EntertaimentPaymentMethodService(ILogger<EntertaimentPaymentMethodService> logger, KafkaRequestService kafkaRequestService) : IEntertaimentPaymentMethodService
    {
        private readonly ILogger<EntertaimentPaymentMethodService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_METHOD_REQUEST_TOPIC");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_METHOD_RESPONSE_TOPIC");
        public async Task<AddPaymendMethodEntertainmentResponse> AddPaymentMethod(AddPaymentMethodEntertaimentRequest request)
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
                    return _kafkaRequestService.GetMessage<AddPaymendMethodEntertainmentResponse>(messageId.ToString(),responseTopic);
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
        public async Task<UpdatePaymentMethodEntertainmentResponse> UpdatePaymentMethod(UpdatePaymentMethodEntertaimentRequest request)
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
                    return _kafkaRequestService.GetMessage<UpdatePaymentMethodEntertainmentResponse>(messageId.ToString(),responseTopic); 
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
        public async Task<GetPaymentMethodEntertaimentResponse> GetPaymentMethod(GetPaymentMethodEntertainmentRequest request)
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
                    return _kafkaRequestService.GetMessage<GetPaymentMethodEntertaimentResponse>(messageId.ToString(),responseTopic);
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
        public async Task<RemovePaymentMethodEntertaimentResponse> RemovePaymentMethod(RemovePaymentMethodEntertaimentRequest request)
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
                    return _kafkaRequestService.GetMessage<RemovePaymentMethodEntertaimentResponse>(messageId.ToString(),responseTopic);
                
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