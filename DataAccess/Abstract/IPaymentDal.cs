using Core.DataAccess;
using Entities.Payments;

namespace DataAccess.Abstract
{
    public interface IPaymentDal : IGenericRepository<Payment>
    {
    }

}
