using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.Tag.Requests;
using ApiGatewayService.Models.TourService.Models.Tag.Responses;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;
using TourService.Models.Tag.Responses;

namespace ApiGatewayService.Services.TourService.Tags
{
    public class TourTagsService(ILogger<TourTagsService> logger, KafkaRequestService kafkaRequestService) : ITourTagsService
    {
        private readonly ILogger<TourTagsService> _logger = logger;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        private readonly string requestTopic = Environment.GetEnvironmentVariable("TOUR_TAGS_REQUEST_TOPIC");
        private readonly string responseTopic = Environment.GetEnvironmentVariable("TOUR_TAGS_RESPONSE_TOPIC");
        public async Task<AddTagResponse> AddTag(AddTagRequest addTagRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(addTagRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("addTag")),
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
                    return _kafkaRequestService.GetMessage<AddTagResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<GetTagResponse> GetTag(GetTagRequest getTagRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(getTagRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getTag")),
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
                    return _kafkaRequestService.GetMessage<GetTagResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<GetTagsResponse> GetTags(GetTagsRequest getTagsRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(getTagsRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getAllTags")),
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
                    return _kafkaRequestService.GetMessage<GetTagsResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<RemoveTagResponse> RemoveTag(RemoveTagRequest removeTagRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(removeTagRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("removeTag")),
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
                    return _kafkaRequestService.GetMessage<RemoveTagResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<UpdateTagResponse> UpdateTag(UpdateTagRequest updateTagRequest)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(updateTagRequest),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("updateTag")),
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
                    return _kafkaRequestService.GetMessage<UpdateTagResponse>(messageId.ToString(),responseTopic);
                }
                else
                {
                    _logger.LogError("Message not sent :{messageId}",messageId.ToString());
                    return null;
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