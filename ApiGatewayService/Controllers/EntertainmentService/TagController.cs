using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    public Task<IActionResult> GetTag()
    {
        throw new NotImplementedException();
    }
    public Task<IActionResult> GetTags()
    {
        throw new NotImplementedException();
    }
}