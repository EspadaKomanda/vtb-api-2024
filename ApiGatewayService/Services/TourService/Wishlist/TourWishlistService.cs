using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.Models.Wishlist.Requests;
using TourService.Models.Wishlist.Responses;
using WishListService.Models.Wishlist.Responses;

namespace ApiGatewayService.Services.TourService.Wishlist
{
    public class TourWishlistService(ILogger<TourWishlistService> logger, KafkaRequestService kafkaRequestService) : ITourWishlistService
    {
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly ILogger<TourWishlistService> _logger = logger;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("TOUR_WISHLIST_REQUEST_TOPIC");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("TOUR_WISHLIST_RESPONSE_TOPIC");
        public async Task<GetWishlistedToursResponse> GetWishlistedTours(GetWishlistedToursRequest getWishlistedToursRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(getWishlistedToursRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getWishlistedTours")),
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
                    return _kafkaRequestService.GetMessage<GetWishlistedToursResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in GetWishlistedTours");
                throw new Exception("Error in GetWishlistedTours");
            }
        }
        public async Task<UnwishlistTourResponse> UnwishlistTour(UnwishlistTourRequest unwishlistTourRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(unwishlistTourRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("unwishlistTour")),
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
                    return _kafkaRequestService.GetMessage<UnwishlistTourResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in UnwishlistTour");
                throw new Exception("Error in UnwishlistTour");
            }
        }
        public async Task<WishlistTourResponse> WishlistTour(WishlistTourRequest wishlistTourRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(wishlistTourRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("wishlistTour")),
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
                    return _kafkaRequestService.GetMessage<WishlistTourResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    throw new Exception("Message not sent");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error in WishlistTour");
                throw new Exception("Error in WishlistTour");
            }
        }
    
    }
}