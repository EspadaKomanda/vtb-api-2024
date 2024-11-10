using ApiGatewayService.Models.AuthService.Models;

namespace ApiGatewayService.Services.Jwt;

public class JwtService(ILogger<JwtService> logger) : IJwtService
{
    private readonly ILogger<JwtService> _logger = logger;

    public Task<ValidatedUser?> ValidateAccessToken(string token)
    {
        throw new NotImplementedException();
    }

    public Task<ValidatedUser?> ValidateRefreshToken(string token)
    {
        throw new NotImplementedException();
    }
}