using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private  IUserService? _userService;
        protected IUserService UserService => _userService ??= HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;

        protected int GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Int32.Parse(userId);
        }
        protected string GetCurrentUserEmail()
        {
            var email = User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? User.FindFirstValue(ClaimTypes.Email);
            return email;
        }
        protected string GetCurrentUserFullName()
        {
            var fullName = User.FindFirstValue(ClaimTypes.Name);
            return fullName;
        }
        protected string GetCurrentUserType() 
        {
            var userType = User.FindFirstValue(ClaimTypes.Role);
            return userType;
        }

    }
}
