using AutoMapper;
using Core.Entities;
using Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public class UserMappingProfile:Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserDto, AppUser>().ReverseMap();
            CreateMap<UpdateUserDto, AppUser>().ReverseMap();
        }
    }
}
