using ApiGatewayService.Models.AuthService.AccessDataCache.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.AuthService;

[ApiController]
[Route("api/[controller]")]
public class AccessDataCacheController : Controller
{
    public Task<IActionResult> RecacheUser([FromBody] RecacheUserRequest recacheUserRequest)
    {
        throw new NotImplementedException();
    }
}