using ApiGatewayService.Services.TourService.Wishlist;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;
using TourService.Models.Wishlist.Requests;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class WishlistController(ILogger<WishlistController> logger, ITourWishlistService wishlistService) : ControllerBase
{
    private readonly ILogger<WishlistController> _logger = logger;
    private readonly ITourWishlistService _wishlistService = wishlistService;

    [HttpGet]
    [Route("wishlists/user/{userId}/page/{pageId}")]
    public async Task<IActionResult> GetWishlistedTours([FromRoute] long userId, [FromRoute] int pageId)
    {
        GetWishlistedToursRequest request = new() { UserId = userId, Page = pageId };
        try
        {
            var result = await _wishlistService.GetWishlistedTours(request);
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

    [HttpPost]
    [Route("wishlists")]
    public async Task<IActionResult> AddTourToWishlist(WishlistTourRequest request)
    {
        try
        {
            var result = await _wishlistService.WishlistTour(request);
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

    [HttpDelete]
    [Route("wishlists")]
    public async Task<IActionResult> UnwishlistTour(UnwishlistTourRequest request)
    {
        try
        {
            var result = await _wishlistService.UnwishlistTour(request);
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