using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using EntertaimentService.Models.PaymentVariant.Requests;
using EntertaimentService.Models.PaymentVariant.Responses;
using Newtonsoft.Json;
using TourService.Kafka;
namespace ApiGatewayService.Services.EntertaimentService.PaymentVariants
{
    public class EntertaimentPaymentVariant(ILogger<EntertaimentPaymentVariant> logger, KafkaRequestService kafkaRequestService) : IEntertaimentPaymentVariant
    {
        private readonly ILogger<EntertaimentPaymentVariant> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private string requestTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_VARIANT_REQUEST_TOPIC");
        private string responseTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_VARIANT_RESPONSE_TOPIC");
        public async Task<AddPaymentVariantResponse> AddPaymentVariant(AddPaymentVariantEntertaimentRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("addPaymentVariant")),
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
                    return _kafkaRequestService.GetMessage<AddPaymentVariantResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<GetPaymentVariantResponse> GetPaymentVariant(GetPaymentVariantEntertainmentRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("getPaymentVariant")),
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
                    return _kafkaRequestService.GetMessage<GetPaymentVariantResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RemovePaymentVariantResponse> RemovePaymentVariant(RemovePaymentVariantEntertainmentRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("removePaymentVariant")),
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
                    return _kafkaRequestService.GetMessage<RemovePaymentVariantResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<UpdatePaymentVariantResponse> UpdatePaymentVariant(UpdatePaymentVariantEntertaimentRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("updatePaymentVariant")),
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
                    return _kafkaRequestService.GetMessage<UpdatePaymentVariantResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogWarning("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<GetPaymentVariantsResponse> GetPaymentVariants(GetPaymentVariantsEntertainmentRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("getPaymentVariants")),
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
                }
                _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                return _kafkaRequestService.GetMessage<GetPaymentVariantsResponse>(messageId.ToString(),responseTopic);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}