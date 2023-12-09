using AutoMapper;
using Entities.Reservations;

namespace Business.Mapping
{
    public class ReservationMappingProfile : Profile
    {
        public ReservationMappingProfile()
        {
            CreateMap<CreateReservationDto, Reservation>()
                .ForMember(dest => dest.CheckInDate,
                opt => opt.MapFrom(src => DateOnly.FromDateTime((DateTime)src.CheckInDate)))
                .ForMember(dest => dest.CheckOutDate,
                opt => opt.MapFrom(src => DateOnly.FromDateTime((DateTime)src.CheckOutDate)));

            CreateMap<RemoveReservationDto, Reservation>().ReverseMap();
            CreateMap<UpdateReservationDto, Reservation>().ReverseMap();
        }
    }
}
