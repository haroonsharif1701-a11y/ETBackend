using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("services")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            var result = _authService.Login(request);

            if (result == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // Pass the login request object to the JWT service for generating token and then returning that token to the user in the API
            Response.Cookies.Append("token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(8)
            });

            return Ok(result);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestDTO request)
        {
            var result = _authService.Register(request);

            if (result == null || result.IdUser == 0)
                return BadRequest(new { message = result?.Message ?? "Registration failed" });

            return Ok(result);
        }
    }

}
