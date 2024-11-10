using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class TourController : ControllerBase
{
    public Task<IActionResult> GetTour()
    {
        throw new NotImplementedException();
    }
    public Task<IActionResult> GetTours()
    {
        throw new NotImplementedException();
    }
}