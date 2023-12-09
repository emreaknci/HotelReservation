using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reservation
{
    public record ReservationCheckDto
    {
        public int CustomerId { get; init; }
        public int RoomId { get; init; }
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }

    }
}
