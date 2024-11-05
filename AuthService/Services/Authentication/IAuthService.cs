using AuthService.Models;
using AuthService.Models.Authentication.Requests;
using AuthService.Models.Authentication.Responses;

namespace AuthService.Services.Authentication;

/// <summary>
/// AuthService служит для аутентификации пользователя.
/// </summary>
public interface IAuthenticationService
{
    public Task<ValidatedUser> ValidateAccessToken(ValidateAccessTokenRequest request);
    public Task<ValidatedUser> ValidateRefreshToken(ValidateRefreshTokenRequest request);
    public Task<RefreshResponse> Refresh(RefreshRequest request);
}