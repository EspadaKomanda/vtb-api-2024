using ApiGatewayService.Models.AuthService.AccessDataCache.Requests;
using ApiGatewayService.Models.AuthService.AccessDataCache.Responses;
using ApiGatewayService.Models.AuthService.Models;
using AuthService.Services.AccessDataCache;
using TourService.Kafka;

namespace ApiGatewayService.sServices.AuthService.AccessDataCache;

public class AccessDataCacheService(ILogger<AccessDataCacheService> logger, KafkaRequestService kafkaRequestService) : IAccessDataCacheService
{
    private readonly ILogger<AccessDataCacheService> _logger = logger;
    private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;

    public Task<UserAccessData?> Get(string username)
    {
        throw new NotImplementedException();
    }

    public Task<RecacheUserResponse> RecacheUser(RecacheUserRequest user)
    {
        throw new NotImplementedException();
    }

    public Task RecacheUser()
    {
        throw new NotImplementedException();
    }

    public Task<UserAccessData?> RequestAndCacheUser(string username)
    {
        throw new NotImplementedException();
    }

    Task<Models.AccessDataCache.UserAccessData?> IAccessDataCacheService.Get(string username)
    {
        throw new NotImplementedException();
    }

    Task<Models.AccessDataCache.UserAccessData?> IAccessDataCacheService.RequestAndCacheUser(string username)
    {
        throw new NotImplementedException();
    }
}
