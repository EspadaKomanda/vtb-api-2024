using ApiGatewayService.Models.AccessDataCache;

namespace AuthService.Services.AccessDataCache;

/// <summary>
/// IAccessDataCacheService служит для кэширования данных доступа пользователя.
/// </summary>
public interface IAccessDataCacheService
{

    /// <summary>
    /// Возвращает данные доступа пользователя по имени пользователя.
    /// </summary>
    Task<UserAccessData?> Get(string username);

    /// <summary>
    /// Позволяет запросить данные доступа пользователя у UserService, при этом автоматически кеширует их.
    /// </summary>
    Task<UserAccessData?> RequestAndCacheUser(string username);

    /// <summary>
    /// Открытый эндпоинт для микросервисов, который позволяет кешировать данные доступа пользователя (в том числе повторно, если они были обновлены).
    /// </summary>
    Task<RecacheUserResponse> RecacheUser(RecacheUserRequest user);
    
}