using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using EntertaimentService.Models.Wishlist.Requests;
using EntertaimentService.Models.Wishlist.Responses;
using Newtonsoft.Json;
using TourService.Kafka;

namespace ApiGatewayService.Services.EntertaimentService.Wishlist
{
    public class EntertaimentWishlistService(ILogger<EntertaimentWishlistService> logger, KafkaRequestService kafkaRequestService) : IEntertaimentWishlistService
    {
        private readonly ILogger<EntertaimentWishlistService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string _requestTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_WISHLIST_REQUEST_TOPIC");
        private readonly string _responseTopic = Environment.GetEnvironmentVariable("ENTERTAIMENT_WISHLIST_RESPONSE_TOPIC");
        public async Task<GetWishlistedEntertaimentsResponse> GetWishlistedEntertaiments(GetWishlistedEntertaimentsRequest getWishlistedEntertaimentsRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(getWishlistedEntertaimentsRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getWishlistedEntertaiments")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(_requestTopic,message,_responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<GetWishlistedEntertaimentsResponse>(messageId.ToString(),_responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        public async Task<UnwishlistEntertaimentResponse> UnwishlistEntertaiment(UnwishlistEntertaimentRequest unwishlistEntertaimentRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(unwishlistEntertaimentRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("unwishlistEntertaiment")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(_requestTopic,message,_responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<UnwishlistEntertaimentResponse>(messageId.ToString(),_responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        public async Task<WishlistEntertaimentResponse> WishlistEntertaiment(WishlistEntertaimentRequest wishlistEntertaimentRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(wishlistEntertaimentRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("wishlistEntertaiment")),
                        new Header("sender",Encoding.UTF8.GetBytes("apiGatewayService"))
                    }
                };
                if(await _kafkaRequestService.Produce(_requestTopic,message,_responseTopic))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message Recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<WishlistEntertaimentResponse>(messageId.ToString(),_responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    
    }
}