namespace PaymentService.Application.Requests
{
    using PaymentService.Domain.Enums;

    public class TopUpRequest
    {
        public decimal Amount { get; set; }
        // Could allow method selection for TopUp too, defaulting to VNPAY/MOMO
        // For now, simplicity: just amount, assumed simulated top-up or direct
        // But in real world, TopUp also needs a payment gateway. 
        // I'll stick to simple TopUp via generic Payment Gateway flow if needed, 
        // or just a direct "Admin TopUp" style for now, or assume it redirects to a gateway.
        // Let's assume TopUp generates a PaymentUrl just like buying a plan.
        public PaymentMethod Method { get; set; } 
        public string? ReturnUrl { get; set; }
    }
}
