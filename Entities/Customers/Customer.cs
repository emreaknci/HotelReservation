using Core.Entities;
using Entities.Reservations;

namespace Entities.Customers
{
    public class Customer : AppUser
    {
        public ICollection<Reservation>? Reservations { get; set; }

    }

}
