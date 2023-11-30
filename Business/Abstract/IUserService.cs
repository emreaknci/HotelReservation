using Core.Entities;
using Core.Utils.Results;
using Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Result<List<AppUser>> GetAll();
        Task<Result<AppUser>> GetByIdAsync(int id);
        Task<Result<AppUser>> GetByEmailAsync(string email);
        Task<Result<AppUser>> AddAsync(CreateUserDto user,UserType userType);
        Task<Result<UpdateUserDto>> UpdateAsync(UpdateUserDto user);
    }
}
