using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiGatewayService.Models.ourService.Models.Benefits;
using ApiGatewayService.Models.TourService.Models.Benefits;
using ApiGatewayService.Models.TourService.Models.Benefits;
using ApiGatewayService.Models.TourService.Models.Benefits.Responses;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.KafkaException.ConsumerException;

namespace ApiGatewayService.Services.TourService.Benefits
{
    public class TourBenefitsService(ILogger<TourBenefitsService> logger, KafkaRequestService kafkaRequestService) : ITourBenefitsService
    {
        private readonly ILogger<TourBenefitsService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("BENEFIT_TOUR_REQUEST_TOPIC");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("BENEFIT_TOUR_RESPONSE_TOPIC");
               
        public async Task<AddBenefitResponse> AddBenefit(AddBenefitRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("addBenefit")),
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
                    return _kafkaRequestService.GetMessage<AddBenefitResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<GetBenefitResponse> GetBenefit(GetBenefitRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("getBenefit")),
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
                    return _kafkaRequestService.GetMessage<GetBenefitResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<GetBenefitsResponse> GetBenefits(GetBenefitsRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("getBenefits")),
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
                    return _kafkaRequestService.GetMessage<GetBenefitsResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<UpdateBenefitResponse> UpdateBenefit(UpdateBenefitRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("updateBenefit")),
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
                    return _kafkaRequestService.GetMessage<UpdateBenefitResponse>(messageId.ToString(),responseTopic);
                }
                throw new ConsumerException("Message not recieved");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<RemoveBenefitResponse> RemoveBenefit(RemoveBenefitRequest request)
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
                        new Header("method",Encoding.UTF8.GetBytes("removeBenefit")),
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
                    return _kafkaRequestService.GetMessage<RemoveBenefitResponse>(messageId.ToString(),responseTopic);
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