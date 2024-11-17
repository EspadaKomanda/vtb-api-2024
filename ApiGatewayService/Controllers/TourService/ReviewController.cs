using ApiGatewayService.Models.TourService.Models.Review.Requests;
using ApiGatewayService.Models.TourService.TourReview.Requests;
using ApiGatewayService.Services.TourService.Reviews;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(ILogger<ReviewController> logger, ITourReviewsService tourReviewsService) : ControllerBase
{
    private readonly ILogger<ReviewController> _logger = logger;
    private readonly ITourReviewsService _tourReviewsService = tourReviewsService;

    [HttpPost]
    [Route("reviews")]
    public async Task<IActionResult> AddReview(AddReviewRequest request)
    {
        try
        {
            var result = await _tourReviewsService.AddReview(request);
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

    [HttpGet]
    [Route("reviews/{reviewId}")]
    public async Task<IActionResult> GetReview([FromRoute] long reviewId)
    {
        GetReviewRequest request = new() { ReviewId = reviewId };
        try
        {
            var result = await _tourReviewsService.GetReview(request);
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

    [HttpGet]
    [Route("reviews/tour/{tourId}/page/{pageId}")]
    public async Task<IActionResult> GetReviews([FromRoute] long tourId, [FromRoute] int pageId)
    {
        GetReviewsRequest request = new()
        {
            TourId = tourId,
            Page = pageId
        };
        try
        {
            var result = await _tourReviewsService.GetReviews(request);
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
    [Route("reviews")]
    public async Task<IActionResult> RemoveReview(RemoveReviewRequest request)
    {
        try
        {
            var result = await _tourReviewsService.RemoveReview(request);
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

    [HttpPatch]
    [Route("reviews")]
    public async Task<IActionResult> UpdateReview(UpdateReviewRequest request)
    {
        try
        {
            var result = await _tourReviewsService.UpdateReview(request);
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