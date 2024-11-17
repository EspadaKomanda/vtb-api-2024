using ApiGatewayService.Models.UserService.Profile.Requests;
using ApiGatewayService.Services.UserService.Profile;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;
using UserService.Models.Profile.Requests;

namespace ApiGatewayService.Controllers.UserService
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(ILogger<ProfileController> logger, IProfileService profileService) : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger = logger;
        private readonly IProfileService _profileService = profileService;

        [HttpGet]
        [Route("getProfile")]
        public async Task<IActionResult> GetProfile([FromQuery] GetProfileRequest getProfileRequest)
        {
            try
            {
                var result = await _profileService.GetProfile(getProfileRequest);
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
        [Route("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest updateProfileRequest)
        {
            try
            {
                var result = await _profileService.UpdateProfile(updateProfileRequest);
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
        [Route("uploadAvatar")]
        public async Task<IActionResult> UploadAvatar([FromBody] UploadAvatarRequest updateAvatarRequest)
        {
            try
            {
                var result = await _profileService.UploadAvatar(updateAvatarRequest);
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
}
