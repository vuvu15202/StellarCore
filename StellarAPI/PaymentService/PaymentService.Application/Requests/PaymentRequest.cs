using PaymentService.Domain.Enums;

namespace PaymentService.Application.Requests
{
    public class PaymentRequest
    {
        public Guid PlanId { get; set; }
        public PaymentMethod Method { get; set; }
        public string? ReturnUrl { get; set; } // URL to redirect after payment
    }
}
