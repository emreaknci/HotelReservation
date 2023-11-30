using AutoMapper;
using Entities.Rooms;

namespace Business.Mapping
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<CreateRoomDto, Room>().ReverseMap();
            CreateMap<RemoveRoomDto, Room>().ReverseMap();
            CreateMap<UpdateRoomDto, Room>().ReverseMap();
        }
    }
}
