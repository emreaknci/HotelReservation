using Core.DataAccess;
using DataAccess.Abstract;
using Entities.Reservation;
using Entities.Reservations;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class ReservationDal : GenericRepository<Reservation, HotelReservationDbContext>, IReservationDal
    {
        public ReservationDal(HotelReservationDbContext context) : base(context)
        {
            Context = context;
        }
        private HotelReservationDbContext Context { get;set; }
        public List<ReservationListDto> GetReservationListDto(Expression<Func<ReservationListDto, bool>>? filter = null)
        {
            var result = from reservation in Context.Reservations
                         join room in Context.Rooms on reservation.RoomId equals room.Id
                         join hotel in Context.Hotels on room.HotelId equals hotel.Id
                         join user in Context.Users on reservation.CustomerId equals user.Id
                         join payment in Context.Payments on reservation.PaymentId equals payment.Id
                         select new ReservationListDto
                         {
                             Id = reservation.Id,
                             CustomerId = user.Id,
                             Amount = payment.Amount,
                             HotelId = hotel.Id,
                             HotelName = hotel.Name,
                             RoomId = room.Id,
                             RoomName = room.Name,
                             RoomType = room.Type.ToString(),
                             CheckInDate = reservation.CheckInDate,
                             CheckOutDate = reservation.CheckOutDate,
                             PaymentId = payment.Id,
                             PaymentStatus = payment.PaymentStatus.ToString(),                                                       
                         };

            return filter == null
                ? result.ToList()
                : result.Where(filter).ToList();
        }
    }
}
