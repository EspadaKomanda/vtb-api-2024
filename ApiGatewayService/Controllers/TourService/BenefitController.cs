using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class BenefitController : ControllerBase
{
    public Task<IActionResult> GetBenefit()
    {
        throw new NotImplementedException();
    }
    public IQueryable<IActionResult> GetBenefits()
    {
        throw new NotImplementedException();
    }
}