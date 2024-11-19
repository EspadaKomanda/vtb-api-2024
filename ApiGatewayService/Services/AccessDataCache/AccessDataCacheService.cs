using System.Text;
using ApiGatewayService.Models.AuthService.AccessDataCache.Requests;
using ApiGatewayService.Models.AuthService.AccessDataCache.Responses;
using ApiGatewayService.Models.AuthService.Models;
using AuthService.Services.AccessDataCache;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;

namespace ApiGatewayService.sServices.AuthService.AccessDataCache;

public class AccessDataCacheService(ILogger<AccessDataCacheService> logger, KafkaRequestService kafkaRequestService) : IAccessDataCacheService
{
    private readonly ILogger<AccessDataCacheService> _logger = logger;
    private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
    private readonly string requestTopic = Environment.GetEnvironmentVariable("AUTH_ACCESS_DATA_CACHE_REQUESTS");
    private readonly string responseTopic = Environment.GetEnvironmentVariable("AUTH_ACCESS_DATA_CACHE_RESPONSES");
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
    public async Task<Models.AccessDataCache.UserAccessData?> Get(string username)
    {
        try
        {
            return await SendRequest<string,Models.AccessDataCache.UserAccessData?>("getUserAccessData",username);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<RecacheUserResponse> RecacheUser(RecacheUserRequest recacheUserRequest)
    {
        try
        {
            return await SendRequest<RecacheUserRequest,RecacheUserResponse>("recacheUser",recacheUserRequest);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<Models.AccessDataCache.UserAccessData?> RequestAndCacheUser(string username)
    {
        try
        {
            return await SendRequest<string,Models.AccessDataCache.UserAccessData?>("requestAndCacheUser",username);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
