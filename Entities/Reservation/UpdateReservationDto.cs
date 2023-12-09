namespace Entities.Reservations
{
    public record UpdateReservationDto
    {
        public DateOnly? CheckInDate { get; init; }
        public DateOnly? CheckOutDate { get; init; }
        public int? RoomId { get; set; }
        public int? CustomerId { get; set; }

    }
}
