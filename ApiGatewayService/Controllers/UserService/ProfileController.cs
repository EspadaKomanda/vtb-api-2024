using ApiGatewayService.Models.Profile.Requests;
using ApiGatewayService.Services.UserService.Account;
using ApiGatewayService.Services.UserService.Profile;
using Microsoft.AspNetCore.Mvc;

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
            // TODO: implement method
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Route("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest updateProfileRequest)
        {
            // TODO: implement method
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("uploadAvatar")]
        public async Task<IActionResult> UploadAvatar([FromBody] UploadAvatarRequest updateAvatarRequest)
        {
            // TODO: implement method
            throw new NotImplementedException();
        }

        // [HttpGet]
        // [Route("getUsernameAndAvatar")]
        // public async Task<IActionResult> GetUsernameAndAvatar()
        // {
        //     // implement method
        //     throw new NotImplementedException();
        // }
    }
}
