using Core.DataAccess;
using DataAccess.Abstract;
using Entities.Payments;

namespace DataAccess.Concrete
{
    public class PaymentDal : GenericRepository<Payment, HotelReservationDbContext>, IPaymentDal
    {
        public PaymentDal(HotelReservationDbContext context) : base(context)
        {
        }
    }
}
