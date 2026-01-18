using PaymentService.Application.Responses;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Usecases.Strategies
{
    public interface IPaymentStrategy
    {
        PaymentMethod Method { get; }
        Task<PaymentResponse> ExecutePaymentAsync(PaymentTransaction transaction, string? returnUrl);
    }
}
