using ApiGatewayService.Services.TourService.Tours;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;
using TourService.Models.Tour.Requests;
using TourService.Models.Tour.Responses;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class TourController(ILogger<TourController> logger, ITourService tourService) : ControllerBase
{
    private readonly ILogger<TourController> _logger = logger;
    private readonly ITourService _tourService = tourService;
    [HttpPost]
    [Route("tours")]
    public async Task<IActionResult> AddTour(AddTourRequest request)
    {
        try
        {
            var result = await _tourService.AddTour(request);
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
    [Route("tours/{tourId}")]
    public async Task<IActionResult> GetTour([FromRoute] long tourId)
    {
        GetTourRequest request = new() { Id = tourId };
        try
        {
            var result = await _tourService.GetTour(request);
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
    [Route("tours")]
    public async Task<IActionResult> UpdateTour(UpdateTourRequest request)
    {
        try
        {
            var result = await _tourService.UpdateTour(request);
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
    [Route("tours")]
    public async Task<IActionResult> RemoveTour(RemoveTourRequest request)
    {
        try
        {
            var result = await _tourService.RemoveTour(request);
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
    [Route("tours/page/{pageId}")]
    public async Task<IActionResult> GetTours([FromRoute] int pageId)
    {
        GetToursRequest request = new() { Page = pageId };
        try
        {
            var result = await _tourService.GetTours(request);
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
    [Route("linkCategories")]
    public async Task<IActionResult> LinkCategories(LinkCategoriesRequests request)
    {
        try
        {
            var result = await _tourService.LinkCategories(request);
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
    [Route("unlinkCategory")]
    public async Task<IActionResult> UnlinkCategory(UnlinkCategoryRequest request)
    {
        try
        {
            var result = await _tourService.UnlinkCategory(request);
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
    [Route("linkPaymentMethods")]
    public async Task<IActionResult> LinkPaymentMethods(LinkPaymentMethodsRequest request)
    {
        try
        {
            var result = await _tourService.LinkPaymentMethods(request);
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
    [Route("unlinkPaymentMethods")]
    public async Task<IActionResult> UnlinkPaymentMethod(UnlinkPaymentMethodRequest request)
    {
        try
        {
            var result = await _tourService.UnlinkPaymentMethod(request);
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
    [Route("linkTags")]
    public async Task<IActionResult> LinkTags(LinkTagsRequest request)
    {
        try
        {
            var result = await _tourService.LinkTags(request);
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
    [Route("unlinkTag")]
    public async Task<IActionResult> UnlinkTag(UnlinkTagRequest request)
    {
        try
        {
            var result = await _tourService.UnlinkTag(request);
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