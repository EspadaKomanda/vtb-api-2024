using System.Text;
using ApiGatewayService.Models.AuthService.Authentication.Requests;
using ApiGatewayService.Models.AuthService.Authentication.Responses;
using ApiGatewayService.Services.UserService.Account;
using Confluent.Kafka;
using Newtonsoft.Json;
using TourService.Kafka;

namespace ApiGatewayService.Services.AuthService.Auth;

public class AuthService(ILogger<AccountService> logger, KafkaRequestService kafkaRequestService) : IAuthService
{
    private readonly ILogger<AccountService> _logger = logger;
    private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
    private readonly string requestTopic = Environment.GetEnvironmentVariable("AUTH_REQUESTS");
    private readonly string responseTopic = Environment.GetEnvironmentVariable("AUTH_RESPONSES");
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

    public async Task<RefreshResponse> Refresh(RefreshRequest request)
    {
        try
        {
            return await SendRequest<RefreshRequest,RefreshResponse>("refresh",request);
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<ValidateAccessTokenResponse> ValidateAccessToken(ValidateAccessTokenRequest request)
    {
        try
        {
            return await SendRequest<ValidateAccessTokenRequest,ValidateAccessTokenResponse>("validateAccessToken",request);
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<ValidateRefreshTokenResponse> ValidateRefreshToken(ValidateRefreshTokenRequest request)
    {
        try
        {
            return await SendRequest<ValidateRefreshTokenRequest,ValidateRefreshTokenResponse>("validateRefreshToken",request);
        }
        catch(Exception)
        {
            throw;
        }
    }
}
