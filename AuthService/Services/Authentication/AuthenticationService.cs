using AuthService.Exceptions.Auth;
using AuthService.Models;
using AuthService.Models.Authentication.Requests;
using AuthService.Models.Authentication.Responses;
using AuthService.Services.AccessDataCache;
using AuthService.Services.Jwt;

namespace AuthService.Services.Authentication;

// TODO: we need more loggers in this code
public class AuthenticationService(IJwtService jwtService, IAccessDataCacheService accessDataCacheService, ILogger<AuthenticationService> logger) : IAuthenticationService
{
    private readonly IJwtService _jwtService = jwtService;
    private readonly IAccessDataCacheService _adcs = accessDataCacheService;
    private readonly ILogger<AuthenticationService> _logger = logger;

    public async Task<RefreshResponse> Refresh(RefreshRequest request)
    {
        string token = request.RefreshToken;
        try
        {
            _logger.LogDebug("Validating refresh token via jwtService...");
            var user = await _jwtService.ValidateRefreshToken(token);
            
            if (user == null)
            {
                _logger.LogDebug("Token validation was not successful");
                throw new InvalidTokenException("Invalid refresh token");
            }
            
            _logger.LogDebug("Token was successfully validated, retrieving user data from ADCS...");

            // FIXME: seperate error handling for event where user is not present in cache
            var accessData = await _adcs.Get(user.Username) ?? throw new UserNotFoundException($"User not found: {user.Username}");

            _logger.LogDebug("User data received, creating tokens via jwtService...");

            return new RefreshResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(accessData),
                RefreshToken = _jwtService.GenerateRefreshToken(accessData)
            };
        }
        catch (Exception)
        {
            _logger.LogDebug("Refresh token was rejected due to an error");
            throw new InvalidTokenException("Invalid refresh token");
        }
    }

    public async Task<ValidateAccessTokenResponse> ValidateAccessToken(ValidateAccessTokenRequest request)
    {
        string token = request.AccessToken;
        try
        {
            _logger.LogDebug("Validating access token via jwtService...");
            var user = await _jwtService.ValidateAccessToken(token);
            
            if (user == null)
            {
                _logger.LogDebug("Token validation was not successful");
                throw new InvalidTokenException("Invalid access token");
            }
            
            _logger.LogDebug("Token was successfully validated");

            return new ValidateAccessTokenResponse { IsSuccess = true };
        }
        catch (Exception)
        {
            _logger.LogDebug("Refresh token was rejected due to an error");
            throw new InvalidTokenException("Invalid access token");
        }
    }

    public async Task<ValidateRefreshTokenResponse> ValidateRefreshToken(ValidateRefreshTokenRequest request)
    {
        string token = request.RefreshToken;
        try
        {
            _logger.LogDebug("Validating access token via jwtService...");
            var user = await _jwtService.ValidateRefreshToken(token);
            
            if (user == null)
            {
                _logger.LogDebug("Token validation was not successful");
                throw new InvalidTokenException("Invalid refresh token");
            }
            
            _logger.LogDebug("Token was successfully validated");

            return new ValidateRefreshTokenResponse { IsSuccess = true };
        }
        catch (Exception)
        {
            _logger.LogDebug("Refresh token was rejected due to an error");
            throw new InvalidTokenException("Invalid refresh token");
        }
    }
}