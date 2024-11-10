using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    public Task<IActionResult> AddReview()
    {
        throw new NotImplementedException();
    }
    public Task<IActionResult> GetReview()
    {
        throw new NotImplementedException();
    }

    public IQueryable<IActionResult> GetReviews()
    {
        throw new NotImplementedException();
    }
    public Task<IActionResult> RateReview()
    {
        throw new NotImplementedException();
    }
    public IActionResult RemoveReview()
    {
        throw new NotImplementedException();
    }
    public IActionResult UpdateReview()
    {
        throw new NotImplementedException();
    }
}