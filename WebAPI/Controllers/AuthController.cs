using Business.Abstract;
using Core.Entities;
using Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("adminRegister")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterDto dto)
        {
            var result = await _authService.AdminRegister(dto);

            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Message);

        }
        [HttpPost("customerRegister")]
        public async Task<IActionResult> CustomerRegister([FromBody] RegisterDto dto)
        {
            var result = await _authService.CustomerRegister(dto);

            return result.Success
                ? Ok(result.Data)
                : BadRequest(result.Message);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var loginResult = await _authService.Login(dto);

            if (!loginResult.Success)    
                return BadRequest(loginResult.Message);
            
            var tokenResult = _authService.CreateAccessToken(loginResult.Data);

            return tokenResult.Success
                ? Ok(tokenResult.Data)
                : BadRequest(tokenResult.Message);
        }

        [HttpGet("user-exists")]
        public async Task<IActionResult> UserExists([FromQuery] string email)
        {
            var result = await _authService.UserExists(email);

            return result.Success
               ? Ok(result.Data)
               : BadRequest(result.Message);
        }

        [HttpPost("create-access-token")]
        public IActionResult CreateAccessToken([FromBody] AppUser user)
        {
            var result = _authService.CreateAccessToken(user);

            return result.Success
               ? Ok(result.Data)
               : BadRequest(result.Message);
        }
    }
}
