using Microsoft.AspNetCore.Mvc;
using RestfulApiDemo.DTOs;
using RestfulApiDemo.Services;

namespace RestfulApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterUserAsync(request.Username, request.Password);
                var token = _authService.CreateSessionToken(user);

                return Ok(new AuthResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Token = token,
                    Message = "User registered successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
            }
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _authService.LoginUserAsync(request.Username, request.Password);
                var token = _authService.CreateSessionToken(user);

                return Ok(new AuthResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Token = token,
                    Message = "Login successful"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }

        /// <summary>
        /// Logout user
        /// </summary>
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var token = HttpContext.Items["SessionToken"]?.ToString();

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "No active session found" });
                }

                await _authService.LogoutUserAsync(token);

                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during logout", error = ex.Message });
            }
        }
    }
}