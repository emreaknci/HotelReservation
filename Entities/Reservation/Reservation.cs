using Core.Entities;
using Entities.Hotels;
using Entities.Payments;
using Entities.Rooms;

namespace Entities.Reservations
{
    public class Reservation : BaseEntity
    {
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public int CustomerId { get; set; }
        public AppUser? Customer { get; set; }

        public Payment? Payment { get; set; }

    }

}
