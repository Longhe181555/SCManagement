using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SC.Application.Common.Interfaces;
using SC.Application.Common.ViewModels;

namespace SC.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("google-response")]
        public async Task<IActionResult> GoogleResponse([FromBody] TokenViewModel request)
        {
            try
            {
                var token = await _accountService.HandleLoginWithGoogleAsync(request);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("facebook-response")]
        public async Task<IActionResult> FacebookResponse([FromBody] TokenViewModel request)
        {
            try
            {
                var token = await _accountService.HandleLoginWithFacebookAsync(request);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await _accountService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Register), new { id = model.Username }, new { message = "User created successfully" });
            }
            else
            {
                return BadRequest(new { message = "Failed to create user", errors = result.Errors });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            try
            {
                var token = await _accountService.LoginAsync(request);
                if(token == null)
                {
                    return Unauthorized(new { message = "Login unsuccessful"});
                }
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("check")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> checkToken()
        {
             return Ok();
        }
    }
}
