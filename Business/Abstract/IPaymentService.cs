using Core.Entities;
using Core.Utils.Results;
using Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPaymentService
    {
        Result<List<Payment>> GetAll();
        Task<Result<Payment>> GetByIdAsync(int id);
        Result<PaginationResult<Payment>> GetAllPagination(BasePaginationRequest req);
        Task<Result<Payment>> PayAsync(CreatePaymentDto payment, DateTime checkOutDate, DateTime checkInDate);
        Task<Result<decimal>> GetAllTimeEarnings();

    }
}
