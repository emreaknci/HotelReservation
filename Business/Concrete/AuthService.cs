using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using Core.Utils.Security.Hashing;
using Core.Utils.Security.JWT;
using Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHandler _tokenHandler;
        public AuthService(IUserService userService, ITokenHandler tokenHandler)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;
        }

        public Task<Result<AppUser>> AdminRegister(RegisterDto dto)
        {
            return Register(dto, UserType.Admin);
        }

        public Result<AccessToken> CreateAccessToken(AppUser user)
        {
            var token = _tokenHandler.CreateAccessToken(user);
            return Result<AccessToken>.SuccessResult(token);
        }

        public Task<Result<AppUser>> CustomerRegister(RegisterDto dto)
        {
            return Register(dto, UserType.Customer);
        }

        public async Task<Result<AppUser>> Login(LoginDto dto)
        {
            var userToCheck = await _userService.GetByEmailAsync(dto.Email);
            if (!userToCheck.Success)
                return Result<AppUser>.FailureResult(userToCheck.Message);

            var result = !HashingHelper.VerifyPasswordHash(dto.Password!, userToCheck.Data.PasswordHash!,
                                   userToCheck.Data.PasswordSalt!);

            return result
                ? Result<AppUser>.FailureResult("Doğru şifreyi girdiğinizden emin olun")
                : Result<AppUser>.SuccessResult(userToCheck.Data, "Giriş başarılı");
        }

        public async Task<Result<bool>> UserExists(string email)
        {
            var result = await _userService.GetByEmailAsync(email);

            return result.Success
                ? Result<bool>.SuccessResult(result.Success)
                : Result<bool>.FailureResult(result.Message);
        }
        private async Task<Result<AppUser>> Register(RegisterDto dto, UserType userType)
        {
            var userExists = await UserExists(dto.Email);
            if (userExists.Success)
                return Result<AppUser>.FailureResult("Bu mail başka bir kullanıcıya ait");

            HashingHelper.CreatePasswordHash(dto.Password!, out var hash, out var salt);
            var user = new CreateUserDto
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            var result = await _userService.AddAsync(user, userType);

            return result;
        }

    }
}
