using ApiGatewayService.Services.EntertaimentService.Entertaiments;
using EntertaimentService.Models.Entertaiment.Requests;
using EntertaimentService.Models.Tour.Requests;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class EntertaimentController(ILogger<EntertaimentController> logger, IEntertaimentService entertainmentService) : ControllerBase
{
    private readonly ILogger<EntertaimentController> _logger = logger;
    private readonly IEntertaimentService _entertainmentService = entertainmentService;
    [HttpPost]
    [Route("entertainments")]
    public async Task<IActionResult> AddEntertainment(AddEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentService.AddEntertaiment(request);
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
    [Route("entertainments/{entertainmentId}")]
    public async Task<IActionResult> GetEntertainment([FromRoute] long entertainmentId)
    {
        GetEntertaimentRequest request = new() { Id = entertainmentId };
        try
        {
            var result = await _entertainmentService.GetEntertaiment(request);
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
    [Route("entertainments")]
    public async Task<IActionResult> UpdateEntertainment(UpdateEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentService.UpdateEntertaiment(request);
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
    [Route("entertainments")]
    public async Task<IActionResult> RemoveEntertainment(RemoveEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentService.RemoveEntertaiment(request);
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
    [Route("entertainments/page/{pageId}")]
    public async Task<IActionResult> GetEntertainments([FromRoute] int pageId)
    {
        GetEntertaimentsRequest request = new() { Page = pageId };
        try
        {
            var result = await _entertainmentService.GetEntertaiments(request);
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
    public async Task<IActionResult> LinkCategories(LinkCategoriesEntertaimentRequests request)
    {
        try
        {
            var result = await _entertainmentService.LinkCategories(request);
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
    public async Task<IActionResult> UnlinkCategory(UnlinkCategoryEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentService.UnlinkCategory(request);
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
    public async Task<IActionResult> LinkPaymentMethods(LinkPaymentMethodsEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentService.LinkPaymentMethods(request);
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
    public async Task<IActionResult> UnlinkPaymentMethod(UnlinkPaymentMethodEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentService.UnlinkPaymentMethod(request);
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