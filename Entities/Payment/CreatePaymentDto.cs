namespace Entities.Payments
{
    public record CreatePaymentDto
    {
        public decimal Amount { get; init; }
        public DateTime? PaymentDate { get; init; }
        public string? CVV { get; init; }
        public string? CardNumber { get; init; }
        public string? CardHolderName { get; init; }
        public DateTime? ExpirationDate { get; init; }

        public bool IsCardValid()
        {
            if (string.IsNullOrEmpty(CVV) || CVV.Length != 3 || !CVV.All(char.IsDigit))
            {
                return false;
            }

            if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length != 16 || !CardNumber.All(char.IsDigit))
            {
                return false;
            }

            if (string.IsNullOrEmpty(CardHolderName) || CardHolderName.Any(char.IsDigit))
            {
                return false;
            }

            if (ExpirationDate == null || ExpirationDate <= DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }


}
