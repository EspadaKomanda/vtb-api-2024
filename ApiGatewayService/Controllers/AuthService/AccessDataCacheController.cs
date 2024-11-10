using ApiGatewayService.Models.AuthService.AccessDataCache.Requests;
using ApiGatewayService.Models.AuthService.AccessDataCache.Responses;
using AuthService.Services.AccessDataCache;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.AuthService;

[ApiController]
[Route("api/[controller]")]
public class AccessDataCacheController(ILogger<AccessDataCacheController> logger, IAccessDataCacheService accessDataCacheService) : ControllerBase
{
    private readonly ILogger<AccessDataCacheController> _logger = logger;
    private readonly IAccessDataCacheService _accessDataCacheService = accessDataCacheService;

    [HttpPost("recacheUser")]
    public async Task<RecacheUserResponse> RecacheUser([FromBody] RecacheUserRequest request)
    {
        throw new NotImplementedException();
    }
}