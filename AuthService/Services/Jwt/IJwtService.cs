using AuthService.Services.Models;

namespace AuthService.Services.Jwt;

public interface IJwtService
{
    /// <summary>
    /// Генерирует access и refresh токены.
    /// </summary>
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    User ValidateAccessToken(string token);
    User ValidateRefreshToken(string token);
}