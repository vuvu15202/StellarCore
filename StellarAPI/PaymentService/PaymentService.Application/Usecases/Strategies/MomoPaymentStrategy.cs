using PaymentService.Application.Requests;
using PaymentService.Application.Responses;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Usecases.Strategies
{
    public class MomoPaymentStrategy : IPaymentStrategy
    {
        public PaymentMethod Method => PaymentMethod.Momo;

        public async Task<PaymentResponse> ExecutePaymentAsync(PaymentTransaction transaction, string? returnUrl)
        {
            // Simulate MOMO integration
            var mockUrl = $"https://test-payment.momo.vn/v2/gateway/api/create?amount={transaction.Amount}&orderId={transaction.OrderId}&returnUrl={returnUrl}";

            return await Task.FromResult(new PaymentResponse
            {
                IsSuccess = true,
                Message = "Redirect to MOMO to complete payment",
                PaymentUrl = mockUrl,
                OrderId = transaction.OrderId
            });
        }
    }
}
