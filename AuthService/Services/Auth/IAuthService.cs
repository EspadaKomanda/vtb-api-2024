using AuthService.Models.Auth.Requests;
using AuthService.Models.Auth.Responses;

namespace AuthService.Services.Auth;

/// <summary>
/// AuthService служит для аутентификации пользователя.
/// </summary>
public interface IAuthService
{
    Task<RefreshResponse> Refresh(RefreshRequest request);
}