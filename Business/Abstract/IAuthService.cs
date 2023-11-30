using Core.Entities;
using Core.Utils.Results;
using Core.Utils.Security.JWT;
using Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<Result<AppUser>> CustomerRegister(RegisterDto dto);
        Task<Result<AppUser>> AdminRegister(RegisterDto dto);
        Task<Result<AppUser>> Login(LoginDto dto);
        Task<Result<bool>> UserExists(string email);
        Result<AccessToken> CreateAccessToken(AppUser user);
    }
}
