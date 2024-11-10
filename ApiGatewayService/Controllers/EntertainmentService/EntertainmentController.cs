using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]")]
public class TourController : ControllerBase
{
    public Task<IActionResult> GetEntertainment()
    {
        throw new NotImplementedException();
    }
    public Task<IActionResult> GetEntertainments()
    {
        throw new NotImplementedException();
    }
}