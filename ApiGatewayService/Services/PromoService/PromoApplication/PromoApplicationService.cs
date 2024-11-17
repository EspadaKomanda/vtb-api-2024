using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiGatewayService.Models.PromoService.PromoApplication.Requests;
using ApiGatewayService.Models.PromoService.PromoApplication.Responses;
using ApiGatewayService.Services.PromoService.PromoApplication;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;

namespace ApiGatewayService.Services.PromoService
{
    public class PromoApplicationService(ILogger<PromoApplicationService> logger, KafkaRequestService kafkaRequestService) : IPromoApplicationService
    {
        private readonly ILogger<PromoApplicationService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("PROMO_APPLICATION_REQUESTS");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("PROMO_APPLICATION_RESPONSES");
        private readonly string serviceName = "apiGatewayService";
        private async Task<Q> SendRequest<T,Q>(string methodName, T request)
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
                        new Header("method",Encoding.UTF8.GetBytes(methodName)),
                        new Header("sender",Encoding.UTF8.GetBytes(serviceName))
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
                    return _kafkaRequestService.GetMessage<Q>(messageId.ToString(),responseTopic);
                }
                throw new Exception("Message not recieved");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<RegisterPromoUseResponse> RegisterPromoUse(RegisterPromoUseRequest request)
        {
            try
            {
                return await SendRequest<RegisterPromoUseRequest,RegisterPromoUseResponse>("registerPromoUse",request);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task<ValidatePromocodeApplicationResponse> ValidatePromocodeApplication(ValidatePromocodeApplicationRequest request)
        {
            try
            {
                return await SendRequest<ValidatePromocodeApplicationRequest,ValidatePromocodeApplicationResponse>("validatePromocodeApplication",request);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task<GetMyPromoApplicationsResponse> GetMyPromoApplications(GetMyPromoApplicationsRequest request)
        {
            try
            {
                return await SendRequest<GetMyPromoApplicationsRequest,GetMyPromoApplicationsResponse>("getMyPromoApplications",request);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}