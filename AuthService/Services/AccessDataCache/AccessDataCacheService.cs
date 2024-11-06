using System.Text.Json;
using AuthService.Models.AccessDataCache.Requests;
using AuthService.Models.AccessDataCache.Responses;
using AuthService.Services.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace AuthService.Services.AccessDataCache;

public class AccessDataCacheService(IDistributedCache cache, ILogger<AccessDataCacheService> logger) : IAccessDataCacheService
{
    private readonly IDistributedCache _cache = cache;
    private readonly ILogger<AccessDataCacheService> _logger = logger;

    public async Task<UserAccessData?> Get(string username)
    {
        _logger.LogDebug("Retrieving user from cache...");
        try 
        {
            var userBytes = await _cache.GetAsync(username);

            return userBytes == null ? null : JsonSerializer.Deserialize<UserAccessData>(userBytes);
        }
        catch (JsonException)
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
            await _cache.SetStringAsync(user.Username, JsonSerializer.Serialize(user));
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
                
                // TODO: Implement communication with userservice
                throw new NotImplementedException("Communication with userservice not implemented");
            }

            return userBytes == null ? null : JsonSerializer.Deserialize<UserAccessData>(userBytes);
        }
        catch (JsonException)
        {
            // Clear cache if deserialization fails
            _logger.LogError("Deserialization failed, removing user from cache...");
            await _cache.RemoveAsync(username);
            throw;
        }
    }
}
