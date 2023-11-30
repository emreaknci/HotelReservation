using Entities.Payments;

namespace Entities.Reservations
{
    public record CreateReservationDto
    {
        public DateTime? CheckInDate { get; init; }
        public DateTime? CheckOutDate { get; init; }
        public int HotelId { get; init; }
        public int RoomId { get; init; }
        public int CustomerId { get; init; }
        public CreatePaymentDto paymentDto { get; init; }
    }
}
