using Entities.Hotels;

namespace Entities.Rooms
{
    public class Room
    {
        public int Id { get; set; }
        public string? Type { get; set; }

        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }

}
