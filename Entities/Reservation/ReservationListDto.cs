using Core.Entities;
using Entities.Hotels;
using Entities.Payments;
using Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reservation
{
    public record ReservationListDto
    {
        public int? Id { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int? HotelId { get; set; }
        public string? HotelName { get; set; }
        public int? RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? RoomType { get; set; }
        public int? CustomerId { get; set; }
        public int? PaymentId { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
