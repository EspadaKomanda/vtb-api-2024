using ApiGatewayService.Models.TourService.Models.Photos.Requests;
using ApiGatewayService.Services.TourService.Photos;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class PhotoController(ILogger<PhotoController> logger, ITourPhotoService photoService) : ControllerBase
{
    private readonly ILogger<PhotoController> _logger = logger;
    private readonly ITourPhotoService _photoService = photoService;

    [HttpPost]
    [Route("photos")]
    public async Task<IActionResult> AddPhoto(AddPhotoRequest request)
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
    public async Task<IActionResult> RemovePhoto(RemovePhotoRequest request)
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
    public async Task<IActionResult> UpdatePhoto(UpdatePhotoRequest request)
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
        GetPhotoRequest request = new() {PhotoId = photoId};
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
    [Route("photos/tour/{tourId}")]
    public async Task<IActionResult> GetPhotos([FromRoute] long tourId)
    {
        GetPhotosRequest request = new() {TourId = tourId};
        try
        {
            var result = _photoService.GetPhotos(request);
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