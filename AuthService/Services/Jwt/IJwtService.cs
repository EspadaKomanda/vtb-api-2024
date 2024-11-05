using AuthService.Models;
using AuthService.Services.Models;

namespace AuthService.Services.Jwt;

/// <summary>
/// IJwtService служит для работы с JWT токенами.
/// </summary>
public interface IJwtService
{
    string GenerateAccessToken(UserAccessData user);
    string GenerateRefreshToken(UserAccessData user);
    ValidatedUser? ValidateAccessToken(string token);
    ValidatedUser? ValidateRefreshToken(string token);
}