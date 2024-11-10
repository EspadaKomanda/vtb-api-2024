using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    Task<IActionResult> GetTag()
    {
        throw new NotImplementedException();
    }
    IQueryable<IActionResult> GetTags()
    {
        throw new NotImplementedException();
    }
}