namespace Entities.Payments
{
    public record PaymentDto
    {
        public List<Payment>? Payments { get; init; }
        public decimal? TotalAmount { get; init; }
        public int? TotalPaymentCount { get; init; }
        public int? TotalPaidCount { get; init; }
        public int? TotalCanceledCount { get; init; }
    }

}
