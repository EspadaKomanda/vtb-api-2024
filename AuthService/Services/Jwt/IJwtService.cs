namespace AuthService.Services.Jwt;

public interface IJwtService
{
    string GenerateAccessToken();
    string GenerateRefreshToken();
    bool ValidateAccessToken(string token);
    bool ValidateRefreshToken(string token);
}