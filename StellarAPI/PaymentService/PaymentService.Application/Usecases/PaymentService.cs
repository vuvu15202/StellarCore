using PaymentService.Application.Requests;
using PaymentService.Application.Responses;
using PaymentService.Application.Responses;
using PaymentService.Application.Usecases.Strategies;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Services.Persistence;

namespace PaymentService.Application.Usecases
{
    public class PaymentService
    {
        private readonly PaymentPersistence _paymentRepository;
        private readonly IPaymentStrategyFactory _strategyFactory;
        private readonly WalletService _walletService;

        public PaymentService(
            PaymentPersistence paymentRepository, 
            IPaymentStrategyFactory strategyFactory,
            WalletService walletService)
        {
            _paymentRepository = paymentRepository;
            _strategyFactory = strategyFactory;
            _walletService = walletService;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(Guid userId, PaymentRequest request)
        {
            // 1. Create Transaction
            var orderId = Guid.NewGuid().ToString();
            // TODO: Fetch Plan price from UserService instead of trusting request (if amount was here)
            // For now, assume PlanId is the only info we have and we need to fetch price. 
            // Since we don't have PlanService connected yet, I will use a dummy price or pass it in DTO (bad practice but works for structural demo).
            // Let's assume validation happens before or we trust the flow for this step.
            // I'll add a placeholder price.
            decimal amount = 10000; // Mock amount

            var transaction = new PaymentTransaction
            {
                OrderId = orderId,
                UserId = userId,
                PlanId = request.PlanId,
                Amount = amount,
                Method = request.Method,
                Status = TransactionStatus.Pending,
                Description = $"Payment for Plan {request.PlanId}",
                CreatedAt = DateTime.UtcNow
            };

            _paymentRepository.Save(transaction);

            // 2. Get Strategy
            var strategy = _strategyFactory.GetStrategy(request.Method);

            // 3. Execute
            var response = await strategy.ExecutePaymentAsync(transaction, request.ReturnUrl);

            // 4. Update if immediate success (like Wallet)
            if (response.IsSuccess && request.Method == PaymentMethod.Wallet)
            {
                transaction.Status = TransactionStatus.Success;
                transaction.TransactionId = response.TransactionId;
                _paymentRepository.Save(transaction);
                
                // TODO: Call UserService to activate subscription
            }

            return response;
        }

        public async Task<PaymentResponse> ProcessTopUpAsync(Guid userId, TopUpRequest request)
        {
            var orderId = Guid.NewGuid().ToString();
            var transaction = new PaymentTransaction
            {
                OrderId = orderId,
                UserId = userId,
                PlanId = Guid.Empty, // 0 for TopUp
                Amount = request.Amount,
                Method = request.Method,
                Status = TransactionStatus.Pending,
                Description = $"TopUp Wallet",
                CreatedAt = DateTime.UtcNow
            };

            _paymentRepository.Save(transaction);

            var strategy = _strategyFactory.GetStrategy(request.Method);
            var response = await strategy.ExecutePaymentAsync(transaction, request.ReturnUrl);

            return response;
        }

        public async Task HandleCallbackAsync(string transactionId, bool success)
        {
            // This is generic, handling async callbacks from VNPAY/MOMO
            // Need to search by OrderId usually
            var transaction = await _paymentRepository.GetByOrderIdAsync(transactionId);
            if (transaction == null) return;

            if (success)
            {
                transaction.Status = TransactionStatus.Success;
                // If it was a Plan payment
                if (transaction.PlanId != Guid.Empty)
                {
                    // Call UserService to subscribe
                }
                // If it was a TopUp
                else
                {
                   await _walletService.TopUpBalanceAsync(transaction.UserId, transaction.Amount, "TopUp from generic gateway");
                }
            }
            else
            {
                transaction.Status = TransactionStatus.Failed;
            }
            
            _paymentRepository.Save(transaction);
        }
    }
}
