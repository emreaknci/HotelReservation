using Core.Entities;
using Entities.Reservations;

namespace Entities.Payments
{
    public class Payment:BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }

}
