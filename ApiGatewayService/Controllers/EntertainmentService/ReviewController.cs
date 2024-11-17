using ApiGatewayService.Services.EntertaimentService.Reviews;
using EntertaimentService.Models.Review.Requests;
using EntertaimentService.TourReview.Requests;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class ReviewController(ILogger<ReviewController> logger, IEntertaimentReviewsService entertainmentReviewsService) : ControllerBase
{
    private readonly ILogger<ReviewController> _logger = logger;
    private readonly IEntertaimentReviewsService _entertainmentReviewsService = entertainmentReviewsService;

    [HttpPost]
    [Route("reviews")]
    public async Task<IActionResult> AddReview(AddReviewEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentReviewsService.AddReview(request);
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
        GetReviewEntertainmentRequest request = new() { ReviewId = reviewId };
        try
        {
            var result = await _entertainmentReviewsService.GetReview(request);
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
    [Route("reviews/entertainment/{entertainmentId}/page/{pageId}")]
    public async Task<IActionResult> GetReviews([FromRoute] long entertainmentId, [FromRoute] int pageId)
    {
        GetReviewsEntertaimentRequest request = new()
        {
            EntertaimentId = entertainmentId,
            Page = pageId
        };
        try
        {
            var result = await _entertainmentReviewsService.GetReviews(request);
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
    public async Task<IActionResult> RemoveReview(RemoveReviewEntertainmentRequest request)
    {
        try
        {
            var result = await _entertainmentReviewsService.RemoveReview(request);
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
    public async Task<IActionResult> UpdateReview(UpdateReviewEntertainmentRequest request)
    {
        try
        {
            var result = await _entertainmentReviewsService.UpdateReview(request);
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