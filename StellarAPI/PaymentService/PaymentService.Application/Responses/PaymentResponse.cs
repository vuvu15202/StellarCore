namespace PaymentService.Application.Responses
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? PaymentUrl { get; set; } // For VNPAY/MOMO
        public string TransactionId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
    }
}
