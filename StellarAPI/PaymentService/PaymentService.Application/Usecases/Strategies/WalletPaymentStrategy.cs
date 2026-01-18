using PaymentService.Application.Requests;
using PaymentService.Application.Responses;
using PaymentService.Application.Usecases;
using PaymentService.Application.Usecases.Strategies;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Usecases.Strategies
{
    public class WalletPaymentStrategy : IPaymentStrategy
    {
        private readonly WalletService _walletService;

        public WalletPaymentStrategy(WalletService walletService)
        {
            _walletService = walletService;
        }

        public PaymentMethod Method => PaymentMethod.Wallet;

        public async Task<PaymentResponse> ExecutePaymentAsync(PaymentTransaction transaction, string? returnUrl)
        {
            var result = await _walletService.DeductBalanceAsync(
                transaction.UserId, 
                transaction.Amount, 
                $"Payment for Plan {transaction.PlanId}"
            );

            if (result)
            {
                return new PaymentResponse
                {
                    IsSuccess = true,
                    Message = "Payment successful",
                    OrderId = transaction.OrderId,
                    TransactionId = Guid.NewGuid().ToString() // Internal tx id
                };
            }
            else
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    Message = "Insufficient balance or invalid wallet",
                    OrderId = transaction.OrderId
                };
            }
        }
    }
}
