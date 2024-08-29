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
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
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
                _logger.LogWarning("Google login failed: Invalid token.");
                return Unauthorized(new { message = "Invalid token" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "An error occurred during Google login.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during Google login.");
                return StatusCode(500, "An error occurred while processing your request.");
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
                _logger.LogWarning("Facebook login failed: Invalid token.");
                return Unauthorized(new { message = "Invalid token" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "An error occurred during Facebook login.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during Facebook login.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                var result = await _accountService.RegisterUserAsync(model);

                if (result.Succeeded)
                {
                    return CreatedAtAction(nameof(Register), new { id = model.Username }, new { message = "User created successfully" });
                }
                else
                {
                    _logger.LogWarning("User registration failed: {Errors}", string.Join(", ", result.Errors));
                    return BadRequest(new { message = "Failed to create user", errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            try
            {
                var token = await _accountService.LoginAsync(request);
                if (token == null)
                {
                    _logger.LogWarning("Login unsuccessful: Invalid credentials for user {Username}.", request.username);
                    return Unauthorized(new { message = "Login unsuccessful" });
                }
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogWarning("Login failed: Invalid credentials for user {Username}.", request.username);
                return Unauthorized(new { message = "Invalid token" });
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {Username}.", request.username);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login for user {Username}.", request.username);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("check")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CheckToken()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while checking the token.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
