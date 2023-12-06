using Core.Entities;
using Entities.Hotels;
using Entities.RoomImages;

namespace Entities.Rooms
{
    public class Room : BaseEntity
    {
        public RoomType Type { get; set; }
        public int? Capacity { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}
