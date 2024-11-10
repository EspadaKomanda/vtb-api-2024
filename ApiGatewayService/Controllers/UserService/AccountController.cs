using ApiGatewayService.Services.UserService.Account;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;
using ApiGatewayService.Models.UserService.Account.Requests;

// TODO: add GetUser and GetUsernameAndAvatar
namespace ApiGatewayService.Controllers.UserService
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(ILogger<AccountController> logger, IAccountService accountService) : ControllerBase
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly IAccountService _accountService = accountService;
        
        [HttpPost]
        [Route("accessAccountData")]
        public async Task<IActionResult> AccessAccountData([FromBody] AccountAccessDataRequest accountAccessDataRequest)
        {
            try
            {
                var account = await _accountService.AccountAccessData(accountAccessDataRequest);
                return Ok(account);
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
        [Route("beginPasswordReset")]
        public async Task<IActionResult> BeginPasswordReset([FromBody] BeginPasswordResetRequest beginPasswordResetRequest)
        {
            try
            {
                var result = await _accountService.BeginPasswordReset(beginPasswordResetRequest);
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
        [Route("beginRegistration")]
        public async Task<IActionResult> BeginRegistration([FromBody] BeginRegistrationRequest beginRegistrationRequest)
        {
            try
            {
                var result = await _accountService.BeginRegistration(beginRegistrationRequest);
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
        [Route("changePassword")]
        // FIXME: authorize
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            try
            {
                var result = await _accountService.ChangePassword(changePasswordRequest);
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
        [Route("completePasswordReset")]
        public async Task<IActionResult> CompletePasswordReset([FromBody] CompletePasswordResetRequest completePasswordResetRequest)
        {
            try
            {
                var result = await _accountService.CompletePasswordReset(completePasswordResetRequest);
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
        [Route("completeRegistration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] CompleteRegistrationRequest completeRegistrationRequest)
        {
            try
            {
                var result = await _accountService.CompleteRegistration(completeRegistrationRequest);
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
        [Route("resendRegistrationCode")]
        public async Task<IActionResult> ResendRegistrationCode([FromBody] ResendRegistrationCodeRequest resendRegistrationCodeRequest)
        {
            try
            {
                var result = await _accountService.ResendRegistrationCode(resendRegistrationCodeRequest);
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
        [Route("resendPasswordResetCode")]
        public async Task<IActionResult> ResendPasswordResetCode([FromBody] ResendPasswordResetCodeRequest resendPasswordResetCodeRequest)
        {
            try
            {
                var result = await _accountService.ResendPasswordResetCode(resendPasswordResetCodeRequest);
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
        [Route("verifyPasswordResetCode")]
        public async Task<IActionResult> VerifyPasswordResetCode([FromBody] VerifyPasswordResetCodeRequest verifyPasswordResetCodeRequest)
        {
            try
            {
                var result = await _accountService.VerifyPasswordResetCode(verifyPasswordResetCodeRequest);
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
        [Route("verfyRegistrationCode")]
        public async Task<IActionResult> VerfyRegistrationCode([FromBody] VerifyRegistrationCodeRequest verifyPasswordResetCodeRequest)
        {
            try
            {
                var result = await _accountService.VerifyRegistrationCode(verifyPasswordResetCodeRequest);
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
        [Route("getUser")]
        // FIXME: authorize
        public async Task<IActionResult> GetUser([FromQuery] GetUserRequest getUserRequest)
        {
            try
            {
                var result = await _accountService.GetUser(getUserRequest);
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