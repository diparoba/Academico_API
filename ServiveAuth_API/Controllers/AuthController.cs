using Microsoft.AspNetCore.Mvc;
using ServiceAuth_API.Models;
using ServiceAuth_API.Services;

namespace ServiceAuth_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceAuth _serviceAuth;
        public AuthController(IServiceAuth serviceAuth)
        {
            _serviceAuth = serviceAuth;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var result = await _serviceAuth.RegisterAsync(user);
            if (result == null)
            {
                return BadRequest("Username already exists");
            }
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _serviceAuth.LoginAsync(user);
            if (result == null)
            {
                return BadRequest("Username or password is incorrect");
            }
            return Ok(new
            {
                token = _serviceAuth.GenerateToken(result),
                UserId = result.Id,
                Name = result.Name,
                Lastname = result.Lastname,
                Email = result.Email,
                Roles = result.Roles
            });
        }
    }
}
