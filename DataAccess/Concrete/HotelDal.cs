using Core.DataAccess;
using DataAccess.Abstract;
using Entities.Hotels;

namespace DataAccess.Concrete
{
    public class HotelDal : GenericRepository<Hotel, HotelReservationDbContext>, IHotelDal
    {
        public HotelDal(HotelReservationDbContext context) : base(context)
        {
        }
    }
   
}
