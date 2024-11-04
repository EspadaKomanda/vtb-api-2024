using AuthService.Services.Models;

namespace AuthService.Services.Jwt;

public class JwtService : IJwtService
{
    public string GenerateAccessToken(User user)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken(User user)
    {
        throw new NotImplementedException();
    }

    public User ValidateAccessToken(string token)
    {
        throw new NotImplementedException();
    }

    public User ValidateRefreshToken(string token)
    {
        throw new NotImplementedException();
    }
}
