using ApiGatewayService.Models.Profile.Requests;
using ApiGatewayService.Services.UserService.Account;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(ILogger<ProfileController> logger, IAccountService userService) : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger = logger;
        private readonly IAccountService _userService = userService;

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
