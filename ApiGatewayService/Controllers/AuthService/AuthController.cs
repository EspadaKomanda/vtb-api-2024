using ApiGatewayService.Models.AuthService.Authentication.Requests;
using ApiGatewayService.Models.AuthService.Authentication.Responses;
using ApiGatewayService.Services.AuthService.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.AuthService;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ILogger<AuthController> logger, IAuthService authService) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IAuthService _authService = authService;

    [HttpPost("validateAccessToken")]
    public async Task<ValidateAccessTokenResponse> ValidateAccessToken([FromBody] ValidateAccessTokenRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("validateRefreshToken")]
    public async Task<ValidateRefreshTokenResponse> ValidateRefreshToken([FromBody] ValidateRefreshTokenRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPost("refresh")]
    public async Task<RefreshResponse> Refresh([FromBody] RefreshRequest request)
    {
        throw new NotImplementedException();
    }
}