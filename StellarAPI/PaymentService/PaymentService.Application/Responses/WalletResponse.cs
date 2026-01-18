namespace PaymentService.Application.Responses
{
    public class WalletResponse
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }
}
