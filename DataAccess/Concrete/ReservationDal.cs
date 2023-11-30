using Core.DataAccess;
using DataAccess.Abstract;
using Entities.Reservations;

namespace DataAccess.Concrete
{
    public class ReservationDal : GenericRepository<Reservation, HotelReservationDbContext>, IReservationDal
    {
        public ReservationDal(HotelReservationDbContext context) : base(context)
        {
        }
    }
}
