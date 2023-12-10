using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using Core.Utils.Security.Hashing;
using DataAccess.Abstract;
using Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IMapper _mapper;

        public UserService(IUserDal userDal, IMapper mapper)
        {
            _userDal = userDal;
            _mapper = mapper;
        }

        public async Task<Result<AppUser>> AddAsync(CreateUserDto user, UserType userType)
        {
            var newUser = _mapper.Map<AppUser>(user);
            newUser.Status = true;
            newUser.UserType = userType;
            newUser = await _userDal.AddAsync(newUser);

            var saved = await _userDal.SaveAsync();
            return saved == 0
                ? Result<AppUser>.FailureResult("Kullanıcı eklenemedi")
                : Result<AppUser>.SuccessResult(newUser, "Kullanıcı eklendi");
        }

        public async Task<Result<bool>> ChangePassword(ChangeUserPasswordDto dto)
        {
            var userToCheck = await GetByIdAsync(dto.Id);
            if (userToCheck == null)
                return Result<bool>.FailureResult(userToCheck.Message);

            var user = userToCheck.Data;
            if (!HashingHelper.VerifyPasswordHash(dto.OldPassword!, user.PasswordHash!, user.PasswordSalt!))
                return Result<bool>.FailureResult("Mevcut şifrenizi doğru girdiğinizden emin olun.");

            if (dto.NewPassword == dto.OldPassword)
                return Result<bool>.FailureResult("Yeni şifre mevcut şifreniz ile aynı olamaz!");



            HashingHelper.CreatePasswordHash(dto.NewPassword!, out var hash, out var salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            _userDal.Update(user);

            var saved = await _userDal.SaveAsync();
            return saved == 0
                ? Result<bool>.FailureResult("Şifre değiştirilemedi")
                : Result<bool>.SuccessResult(true, "Şifre değiştirildi");
        }

        public async Task<Result<bool>> ChangeUserType(int userId)
        {
            var userToCheck = await _userDal.GetByIdAsync(userId);
            if (userToCheck == null)
                return Result<bool>.FailureResult("Kullanıcı bulunamadı");
            userToCheck.UserType = userToCheck.UserType == UserType.Admin ? UserType.Customer : UserType.Admin;
            _userDal.Update(userToCheck);
            var saved = await _userDal.SaveAsync();
            return saved == 0
                ? Result<bool>.FailureResult("Kullanıcı tipi değiştirilemedi")
                : Result<bool>.SuccessResult(true, "Kullanıcı tipi değiştirildi");
        }

        public Result<List<AppUser>> GetAll()
        {
            var users = _userDal.GetAll().ToList();
            return users.Count == 0
                ? Result<List<AppUser>>.FailureResult("Kullanıcı bulunamadı")
                : Result<List<AppUser>>.SuccessResult(users, "Kullanıcılar listelendi");
        }

        public async Task<Result<AppUser>> GetByEmailAsync(string email)
        {
            var user = await _userDal.GetAsync(x => x.Email == email);


            return user == null || user.IsDeleted
                ? Result<AppUser>.FailureResult("Kullanıcı bulunamadı")
                : Result<AppUser>.SuccessResult(user);
        }

        public async Task<Result<AppUser>> GetByIdAsync(int id)
        {
            var user = await _userDal.GetByIdAsync(id);
            return user == null || user.IsDeleted
                ? Result<AppUser>.FailureResult("Kullanıcı bulunamadı")
                : Result<AppUser>.SuccessResult(user);
        }

        public async Task<Result<AppUser>> IsUserAdminAsync(int userId)
        {
            var user = await _userDal.GetByIdAsync(userId);
            return user == null
                ? Result<AppUser>.FailureResult("Kullanıcı bulunamadı")
                : user.UserType == UserType.Admin
                    ? Result<AppUser>.SuccessResult(user)
                    : Result<AppUser>.FailureResult("Kullanıcı Yönetici Değil");
        }

        public async Task<Result<AppUser>> ChangeAccountStatus(int userId)
        {
            var userToCheck = await _userDal.GetByIdAsync(userId);
            if (userToCheck == null)
                return Result<AppUser>.FailureResult("Kullanıcı bulunamadı");

            userToCheck.Status = !userToCheck.Status;
            _userDal.Update(userToCheck);
            var saved = await _userDal.SaveAsync();

            return saved == 0
                ? Result<AppUser>.FailureResult("Kullanıcı hesap durumu değiştirilemedi")
                : Result<AppUser>.SuccessResult(userToCheck, "Kullanıcı hesap durumu değiştirildi");
        }

        public async Task<Result<UpdateUserDto>> UpdateAsync(UpdateUserDto user)
        {
            var userToBeUpdated = _mapper.Map<AppUser>(user);
            userToBeUpdated = _userDal.Update(userToBeUpdated);

            var saved = await _userDal.SaveAsync();
            return saved == 0
                ? Result<UpdateUserDto>.FailureResult("Kullanıcı güncellenemedi")
                : Result<UpdateUserDto>.SuccessResult(user, "Kullanıcı bilgileri güncellendi");
        }
    }
}
