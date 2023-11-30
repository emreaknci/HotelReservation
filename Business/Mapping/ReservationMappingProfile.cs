using AutoMapper;
using Entities.Reservations;

namespace Business.Mapping
{
    public class ReservationMappingProfile : Profile
    {
        public ReservationMappingProfile()
        {
            CreateMap<CreateReservationDto, Reservation>().ReverseMap();
            CreateMap<RemoveReservationDto, Reservation>().ReverseMap();
            CreateMap<UpdateReservationDto, Reservation>().ReverseMap();
        }
    }
}
