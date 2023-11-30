using AutoMapper;
using Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public class HotelMappingProfile:Profile
    {
        public HotelMappingProfile()
        {
            CreateMap<CreateHotelDto, Hotel>().ReverseMap();
            CreateMap<UpdateHotelDto, Hotel>().ReverseMap();
            CreateMap<RemoveHotelDto, Hotel>().ReverseMap();
        }
    }
}
