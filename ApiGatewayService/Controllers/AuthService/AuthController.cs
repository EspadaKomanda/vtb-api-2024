using ApiGatewayService.Models.AuthService.Authentication.Requests;
using ApiGatewayService.Models.AuthService.Authentication.Responses;
using ApiGatewayService.Services.AuthService.Auth;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.AuthService;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ILogger<AuthController> logger, IAuthService authService) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly IAuthService _authService = authService;

    [HttpPost("validateAccessToken")]
    public async Task<IActionResult> ValidateAccessToken([FromBody] ValidateAccessTokenRequest request)
    {
        try
        {
            var result = await _authService.ValidateAccessToken(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("validateRefreshToken")]
    public async Task<IActionResult> ValidateRefreshToken([FromBody] ValidateRefreshTokenRequest request)
    {
        try
        {
            var result = await _authService.ValidateRefreshToken(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        try
        {
            var result = await _authService.Refresh(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }
}