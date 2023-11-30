using Core.DataAccess;
using DataAccess.Abstract;
using Entities.Rooms;

namespace DataAccess.Concrete
{
    public class RoomDal : GenericRepository<Room, HotelReservationDbContext>, IRoomDal
    {
        public RoomDal(HotelReservationDbContext context) : base(context)
        {
        }
    }
}
