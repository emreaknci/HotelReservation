using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using DataAccess.Abstract;
using Entities.Payments;


namespace Business.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentDal _paymentDal;
        private readonly IMapper _mapper;


        public PaymentService(IPaymentDal paymentDal, IMapper mapper)
        {
            _paymentDal = paymentDal;
            _mapper = mapper;
        }

        public async Task<Result<Payment>> PayAsync(CreatePaymentDto payment, DateTime checkOutDate, DateTime checkInDate)
        {
            var amount = payment.Amount * (checkOutDate - checkInDate).Days;
            if (!payment.IsCardValid())
                return Result<Payment>.FailureResult("Kredi kartı bilgileri eksik veya hatalı!");

            var newPayment = _mapper.Map<Payment>(payment);
            newPayment.Amount = (decimal)amount;
            newPayment.PaymentStatus = PaymentStatus.Paid;
            newPayment = await _paymentDal.AddAsync(newPayment);
            await _paymentDal.SaveAsync();
            return newPayment == null
                ? Result<Payment>.FailureResult("Ödeme eklenemedi")
                : Result<Payment>.SuccessResult(newPayment, "Ödeme eklendi");
        }

        public Result<List<Payment>> GetAll()
        {
            var payments = _paymentDal.GetAll().ToList();
            return payments == null || payments.Count == 0
                ? Result<List<Payment>>.FailureResult("Ödeme bulunamadı")
                : Result<List<Payment>>.SuccessResult(payments, "Ödemeler listelendi");
        }

        public Result<PaginationResult<Payment>> GetAllPagination(BasePaginationRequest req)
        {
            var payments = _paymentDal.GetWithPagination(req);

            return payments == null || payments.TotalCount == 0
                ? Result<PaginationResult<Payment>>.FailureResult("Ödeme bulunamadı")
                : Result<PaginationResult<Payment>>.SuccessResult(payments, "Ödemeler listelendi");
        }

        public Task<Result<decimal>> GetAllTimeEarnings()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Payment>> GetByIdAsync(int id)
        {
            var payment = await _paymentDal.GetByIdAsync(id);
            return payment == null
                ? Result<Payment>.FailureResult("Ödeme bulunamadı")
                : Result<Payment>.SuccessResult(payment, "Ödeme bulundu");
        }

        public async Task<Result<string>> CancelPayment(int id)
        {
            var payment =await _paymentDal.GetByIdAsync(id);
            if (payment == null)
                return Result<string>.FailureResult("Ödeme bulunamadı");
            payment.PaymentStatus = PaymentStatus.Canceled;
            await _paymentDal.SaveAsync();
            return Result<string>.SuccessResult("Ödeme iptal edildi");       
        }
    }
}
