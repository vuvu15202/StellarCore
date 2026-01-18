using PaymentService.Application.Requests;
using PaymentService.Application.Responses;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Usecases.Strategies
{
    public class VnPayPaymentStrategy : IPaymentStrategy
    {
        public PaymentMethod Method => PaymentMethod.VnPay;

        public async Task<PaymentResponse> ExecutePaymentAsync(PaymentTransaction transaction, string? returnUrl)
        {
            // Simulate VNPAY integration
            // In real app, build VNPAY URL with hash
            var mockUrl = $"https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?vnp_Amount={transaction.Amount * 100}&vnp_OrderInfo={transaction.Description}&vnp_TxnRef={transaction.OrderId}&vnp_ReturnUrl={returnUrl}";
            
            return await Task.FromResult(new PaymentResponse
            {
                IsSuccess = true, // Initiated
                Message = "Redirect to VNPAY to complete payment",
                PaymentUrl = mockUrl,
                OrderId = transaction.OrderId
            });
        }
    }
}
