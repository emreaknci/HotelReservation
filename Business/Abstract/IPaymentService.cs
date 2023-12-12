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
        Result<PaymentDto> GetAllInDateRange(DateTime startDate, DateTime endDate, PaymentStatus? status);
        Result<PaginationResult<Payment>> GetAllPagination(BasePaginationRequest req);
        Task<Result<Payment>> PayAsync(CreatePaymentDto payment, DateOnly checkOutDate, DateOnly checkInDate);
        Task<Result<string>> CancelPayment(int id);

    }
}
