using Business.Abstract;
using Core.Utils.Results;
using Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordDto dto)
        {
            if(dto.Id != GetCurrentUserId())
                return BadRequest(Result<bool>.FailureResult("Kendi şifreni değiştir kardeşim"));

            var result = await _userService.ChangePassword(dto);

            return result.Success
                ? Ok(result)
                : BadRequest(result);

        }
        [HttpGet("get-all")]
        public  IActionResult GetAll()
        {
            var result = _userService.GetAll();

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }
        [HttpGet("change-user-type")]
        public async Task<IActionResult> ChangeUserType(int userId)
        {
            var result = await _userService.ChangeUserType(userId);

            return result.Success
                ? Ok(result)
                : BadRequest(result); 
        }
        [HttpGet("change-account-status")]
        public async Task<IActionResult> ChangeAccountStatus(int userId)
        {
            var result = await _userService.ChangeAccountStatus(userId);

            return result.Success
                ? Ok(result)
                : BadRequest(result); 
        }
    }
}
