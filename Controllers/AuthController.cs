using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.Request;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("/users")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _authService.GetAllAsync();
            return Ok(results);
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
        {
            var result = await _authService.CreateAsync(signUpRequest);
            return Ok(result);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authService.LoginAsync(loginRequest);
            return Ok(result);
        }
    }
}