using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
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
                : Result<AppUser>.SuccessResult(newUser,"Kullanıcı eklendi");
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
            var user = await _userDal.GetAsync(x=>x.Email==email);
            return user == null
                ? Result<AppUser>.FailureResult("Kullanıcı bulunamadı")
                : Result<AppUser>.SuccessResult(user);
        }

        public async Task<Result<AppUser>> GetByIdAsync(int id)
        {
            var user = await _userDal.GetByIdAsync(id);
            return user==null
                ? Result<AppUser>.FailureResult("Kullanıcı bulunamadı")
                :Result<AppUser>.SuccessResult(user);
        }

        public async Task<Result<UpdateUserDto>> UpdateAsync(UpdateUserDto user)
        {
            var userToBeUpdated = _mapper.Map<AppUser>(user);
            userToBeUpdated = _userDal.Update(userToBeUpdated);
            
            var saved= await _userDal.SaveAsync();
            return saved == 0
                ? Result<UpdateUserDto>.FailureResult("Kullanıcı güncellenemedi")
                : Result<UpdateUserDto>.SuccessResult(user, "Kullanıcı bilgileri güncellendi");
        }
    }
}
