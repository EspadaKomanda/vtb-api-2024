using ApiGatewayService.Models.TourService.Models.Tag.Requests;
using ApiGatewayService.Models.TourService.Models.Tag.Responses;
using ApiGatewayService.Services.TourService.Tags;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class TagController(ILogger<TagController> logger, ITourTagsService tourTagsService) : ControllerBase
{
    private readonly ILogger<TagController> _logger = logger;
    private readonly ITourTagsService _tourTagsService = tourTagsService;

    [Route("tags/{tagId}")]
    [HttpGet]
    public async Task<IActionResult> GetTag([FromRoute] long tagId)
    {
        GetTagRequest request = new() {TagId = tagId};
        try
        {
            var result = await _tourTagsService.GetTag(request);
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
    [Route("tags/page/{pageId}")]
    public async Task<IActionResult> GetTags([FromRoute] int pageId)
    {
        GetTagsRequest request = new() {Page = pageId};
        try
        {
            var result = await _tourTagsService.GetTags(request);
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
    [Route("tags")]
    public async Task<IActionResult> AddTag(AddTagRequest request)
    {
        try
        {
            var result = await _tourTagsService.AddTag(request);
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
    [Route("tags")]
    public async Task<IActionResult> RemoveTag(RemoveTagRequest request)
    {
        try
        {
            var result = await _tourTagsService.RemoveTag(request);
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

    [HttpPut]
    [Route("tags")]
    public async Task<IActionResult> UpdateTag(UpdateTagRequest request)
    {
        try
        {
            var result = await _tourTagsService.UpdateTag(request);
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