using Core.DataAccess;
using DataAccess.Abstract;
using Entities.RoomImages;

namespace DataAccess.Concrete
{
    public class RoomImageDal : GenericRepository<RoomImage, HotelReservationDbContext>, IRoomImageDal
    {
        public RoomImageDal(HotelReservationDbContext context) : base(context)
        {
        }
    }
}
