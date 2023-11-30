using Core.DataAccess;
using Entities.Reservations;

namespace DataAccess.Abstract
{
    public interface IReservationDal : IGenericRepository<Reservation>
    {
    }

}
