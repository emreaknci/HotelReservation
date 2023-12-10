using Core.DataAccess;
using Entities.Reservation;
using Entities.Reservations;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface IReservationDal : IGenericRepository<Reservation>
    {
        List<ReservationListDto> GetReservationListDto(Expression<Func<ReservationListDto, bool>> filter = null);
    }

}
