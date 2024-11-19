using System.Text;
using System.Text.Json;
using ApiGatewayService.Models.UserService.Account.Requests;
using ApiGatewayService.Models.UserService.Account.Responses;
using AuthService.Models.AccessDataCache.Requests;
using AuthService.Models.AccessDataCache.Responses;
using AuthService.Services.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using TourService.Kafka;

namespace AuthService.Services.AccessDataCache;

public class AccessDataCacheService(IDistributedCache cache, ILogger<AccessDataCacheService> logger, KafkaRequestService KafkaRequestService) : IAccessDataCacheService
{
    private readonly IDistributedCache _cache = cache;
    private readonly ILogger<AccessDataCacheService> _logger = logger;
    private readonly KafkaRequestService _kafkaRequestService = KafkaRequestService;
    private readonly string serviceName = "accessDataCacheService";
    private readonly string requestTopic = "user-service-accounts-requests";
    private readonly string responseTopic = "user-service-accounts-responses";
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
    public async Task<UserAccessData?> Get(string username)
    {
        _logger.LogDebug("Retrieving user from cache...");
        try 
        {
            var userBytes = await _cache.GetAsync(username);

            return userBytes == null ? null : System.Text.Json.JsonSerializer.Deserialize<UserAccessData>(userBytes);
        }
        catch (System.Text.Json.JsonException)
        {
            // Clear cache if deserialization fails
            _logger.LogError("Deserialization failed, removing user from cache...");
            await _cache.RemoveAsync(username);
            throw;
        }
        catch (Exception)
        {
            _logger.LogWarning("Failed to retrieve user {user} from cache", username);
            throw;
        }
    }

    public async Task<RecacheUserResponse> RecacheUser(RecacheUserRequest user)
    {
        // TODO: some validation checks

        _logger.LogDebug("Received request to recache user {user}...", user.Username);
        try
        {
            await _cache.SetStringAsync(user.Username, System.Text.Json.JsonSerializer.Serialize(user));
            _logger.LogDebug("User {user} was successfully recached", user.Username);
        }
        catch (Exception)
        {
            _logger.LogWarning("Failed to recache user {user}", user.Username);
            throw;
        }
        return new RecacheUserResponse { IsSuccess = true };
    }

    public async Task<UserAccessData?> RequestAndCacheUser(string username)
    {
        _logger.LogDebug("Retrieving user from cache...");
        try 
        {
            var userBytes = await _cache.GetAsync(username);

            if (userBytes == null)
            {
                _logger.LogDebug("User not found in cache, requesting user from userservice...");
                var response = await SendRequest<AccountAccessDataRequest,AccountAccessDataResponse>("accountAccessData",new AccountAccessDataRequest { Username = username });
            
                if (response == null)
                {
                    throw new Exception($"User not found: {username}");
                }
                return new UserAccessData(){
                    Id = response.UserId,
                    Password = response.Password,
                    Username = response.Username,
                    Role = "user",
                    Salt = response.Salt
                    
                };
            }

            return userBytes == null ? null : System.Text.Json.JsonSerializer.Deserialize<UserAccessData>(userBytes);
        }
        catch (System.Text.Json.JsonException)
        {
            // Clear cache if deserialization fails
            _logger.LogError("Deserialization failed, removing user from cache...");
            await _cache.RemoveAsync(username);
            throw;
        }
    }
}
