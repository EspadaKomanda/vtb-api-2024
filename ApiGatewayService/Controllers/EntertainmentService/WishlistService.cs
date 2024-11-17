using ApiGatewayService.Services.EntertaimentService.Wishlist;
using EntertaimentService.Models.Wishlist.Requests;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class WishlistController(ILogger<WishlistController> logger, IEntertaimentWishlistService wishlistService) : ControllerBase
{
    private readonly ILogger<WishlistController> _logger = logger;
    private readonly IEntertaimentWishlistService _wishlistService = wishlistService;

    [HttpGet]
    [Route("wishlists/user/{userId}/page/{pageId}")]
    public async Task<IActionResult> GetWishlistedEntertainments([FromRoute] long userId, [FromRoute] int pageId)
    {
        GetWishlistedEntertaimentsRequest request = new() { UserId = userId, Page = pageId };
        try
        {
            var result = await _wishlistService.GetWishlistedEntertaiments(request);
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
    public async Task<IActionResult> AddEntertainmentToWishlist(WishlistEntertaimentRequest request)
    {
        try
        {
            var result = await _wishlistService.WishlistEntertaiment(request);
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
    public async Task<IActionResult> UnwishlistEntertainment(UnwishlistEntertaimentRequest request)
    {
        try
        {
            var result = await _wishlistService.UnwishlistEntertaiment(request);
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