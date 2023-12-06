using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using Core.Utils.Security.JWT;
using Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("admin-register")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterDto dto)
        {
            var result = await _authService.AdminRegister(dto);

            return result.Success
                ? Ok(result)
                : BadRequest(result.Message);

        }
        [HttpPost("customer-register")]
        public async Task<IActionResult> CustomerRegister([FromBody] RegisterDto dto)
        {
            var result = await _authService.CustomerRegister(dto);

            return result.Success
                ? Ok(result)
                : BadRequest(result);

        }
    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var loginResult = await _authService.Login(dto);

            if (!loginResult.Success)
                return BadRequest(Result<AccessToken>.FailureResult(loginResult.Message));

            var tokenResult = _authService.CreateAccessToken(loginResult.Data!);

            var response = new LoginResponse(loginResult.Data!.Id, loginResult.Data.Email!, loginResult.Data.FirstName!, loginResult.Data.LastName!, tokenResult.Data!.Token, tokenResult.Data!.Expiration, loginResult.Data.UserType.ToString());
            return tokenResult.Success
                ? Ok(Result<LoginResponse>.SuccessResult(response, loginResult.Message))
                : BadRequest(Result<LoginResponse>.FailureResult(loginResult.Message));
        }

        [HttpGet("user-exists")]
        public async Task<IActionResult> UserExists([FromQuery] string email)
        {
            var result = await _authService.UserExists(email);

            return result.Success
               ? Ok(result)
               : BadRequest(result.Message);
        }

        [HttpPost("create-access-token")]
        public IActionResult CreateAccessToken([FromBody] AppUser user)
        {
            var result = _authService.CreateAccessToken(user);

            return result.Success
               ? Ok(result)
               : BadRequest(result.Message);
        }

        private class LoginResponse
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
            public string UserType { get; set; }

            public LoginResponse(int id, string email, string firstName, string lastName, string token, DateTime expiration, string userType)
            {
                Id = id;
                Email = email;
                FirstName = firstName;
                LastName = lastName;
                Token = token;
                Expiration = expiration;
                UserType = userType;
            }
        }

    }


}
