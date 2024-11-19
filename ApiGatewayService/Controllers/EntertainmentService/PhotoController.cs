using ApiGatewayService.Services.EntertaimentService.Photos;
using EntertaimentService.Models.Photos.Requests;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class PhotoController(ILogger<PhotoController> logger, IEntertaimentPhotoService photoService) : ControllerBase
{
    private readonly ILogger<PhotoController> _logger = logger;
    private readonly IEntertaimentPhotoService _photoService = photoService;

    [HttpPost]
    [Route("photos")]
    public async Task<IActionResult> AddPhoto(AddPhotoEntertaimentRequest request)
    {
        try
        {
            var result = await _photoService.AddPhoto(request);
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
    [Route("photos")]
    public async Task<IActionResult> RemovePhoto(RemovePhotoEntertainmentRequest request)
    {
        try
        {
            var result = await _photoService.RemovePhoto(request);
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
    [Route("photos")]
    public async Task<IActionResult> UpdatePhoto(UpdatePhotoEntertainmentRequest request)
    {
        try
        {
            var result = await _photoService.UpdatePhoto(request);
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
    [Route("photos")]
    public async Task<IActionResult> GetPhoto([FromRoute] long photoId)
    {
        GetPhotoEntertainmentRequest request = new() {PhotoId = photoId};
        try
        {
            var result = await _photoService.GetPhoto(request);
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
    [Route("photos/tour/{entertainmentId}")]
    public async Task<IActionResult> GetPhotos([FromRoute] long entertainmentId)
    {
        GetPhotosEntertaimentRequest request = new() {EntertaimentId = entertainmentId};
        try
        {
            var result = await _photoService.GetPhotos(request);
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