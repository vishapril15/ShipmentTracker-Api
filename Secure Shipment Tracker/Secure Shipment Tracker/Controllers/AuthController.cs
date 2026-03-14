using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Secure_Shipment_Tracker.DTOs;
using Secure_Shipment_Tracker.Helpers;
using Secure_Shipment_Tracker.Services.Auth;

namespace Secure_Shipment_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService auth, ILogger<AuthController> logger)
        {
            _authService = auth;
            _logger = logger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            //if (!ModelState.IsValid)
            //{
            //    _logger.LogWarning("Login attempt with invalid model state");
            //    return BadRequest(ModelState);
            //}
            try
            {
                _logger.LogInformation("Login attempt for username :{UserName} ",login.Username);
                var resp = await _authService.Login(login);
                _logger.LogInformation("User {User} logged in successfully!", login.Username);
                return ApiResponseHelper.Success(resp, "Login Successfull!");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized login attempt for username: {Username}", login.Username);
                return ApiResponseHelper.Error(ex.Message, 401);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for username: {Username}", login.Username);
                return ApiResponseHelper.Error("Something went wrong",500);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            //if (!ModelState.IsValid) {
            //    _logger.LogWarning("Register attempt with invalid model state");
            //    return BadRequest(ModelState); }
            try
            {
                _logger.LogInformation("Admin {AdminUsername} is registering a new user: {NewUsername}", User.Identity?.Name, register.Username);
                await _authService.Register(register);
                _logger.LogInformation("User {NewUsername} registered successfully by Admin {AdminUsername}", register.Username, User.Identity?.Name);
                return ApiResponseHelper.Success<Object>(null, "Registered Successfully!");
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to register user {NewUsername}", register.Username);
                return ApiResponseHelper.Error(ex.Message,StatusCodes.Status400BadRequest);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while registering user {NewUsername}", register.Username);
                return ApiResponseHelper.Error("Something went wrong"+ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
