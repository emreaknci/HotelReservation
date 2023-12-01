namespace Entities.Reservations
{
    public record UpdateReservationDto
    {
        public DateTime? CheckInDate { get; init; }
        public DateTime? CheckOutDate { get; init; }
        public int? RoomId { get; set; }
        public int? CustomerId { get; set; }

    }
}
