using ApiGatewayService.Models.AccessDataCache;
using ApiGatewayService.Models.AuthService.AccessDataCache.Requests;
using ApiGatewayService.Models.AuthService.AccessDataCache.Responses;

namespace AuthService.Services.AccessDataCache;

/// <summary>
/// IAccessDataCacheService служит для кэширования данных доступа пользователя.
/// </summary>
public interface IAccessDataCacheService
{
    /// <summary>
    /// Открытый эндпоинт для микросервисов, который позволяет кешировать данные доступа пользователя (в том числе повторно, если они были обновлены).
    /// </summary>
    Task<RecacheUserResponse> RecacheUser(RecacheUserRequest recacheUserRequest);
    Task<UserAccessData?> Get(string username);
    Task<UserAccessData?> RequestAndCacheUser(string username);
}