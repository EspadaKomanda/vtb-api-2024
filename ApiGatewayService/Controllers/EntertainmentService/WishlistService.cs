using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]")]
public class WishlistController : ControllerBase
{
    public Task<IActionResult> GetWishlists()
    {
        throw new NotImplementedException();
    }
    public Task<IActionResult> AddTourToWishlist()
    {
        throw new NotImplementedException();
    }
    public Task<IActionResult> UnwishlistTour()
    {
        throw new NotImplementedException();
    }
}