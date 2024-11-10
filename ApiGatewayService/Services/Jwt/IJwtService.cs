using ApiGatewayService.Models.AuthService.Models;

namespace ApiGatewayService.Services.Jwt;

/// <summary>
/// IJwtService служит для работы с JWT токенами.
/// </summary>
public interface IJwtService
{
    Task<ValidatedUser?> ValidateAccessToken(string token);
    Task<ValidatedUser?> ValidateRefreshToken(string token);
}