using PaymentService.Application.Requests;
using PaymentService.Application.Responses;
using PaymentService.Application.Responses;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Services.Persistence;

namespace PaymentService.Application.Usecases
{
    public class WalletService
    {
        private readonly WalletPersistence _walletRepository;

        public WalletService(WalletPersistence walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletResponse> GetWalletAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
            {
                // Auto create wallet if not exists - or return empty/inactive
                await CreateWalletAsync(userId);
                wallet = await _walletRepository.GetByUserIdAsync(userId);
            }

            return new WalletResponse
            {
                UserId = wallet!.UserId,
                Balance = wallet.Balance,
                IsActive = wallet.IsActive
            };
        }

        public async Task CreateWalletAsync(Guid userId)
        {
            var existing = await _walletRepository.GetByUserIdAsync(userId);
            if (existing != null) return;

            var wallet = new Wallet
            {
                UserId = userId,
                Balance = 0,
                IsActive = true
            };
            _walletRepository.Save(wallet);
        }

        public async Task<bool> DeductBalanceAsync(Guid userId, decimal amount, string description)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null || !wallet.IsActive || wallet.Balance < amount)
            {
                return false;
            }

            wallet.Balance -= amount;
            _walletRepository.Save(wallet);

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = WalletTransactionType.Payment,
                Status = TransactionStatus.Success,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };
            await _walletRepository.AddTransactionAsync(transaction);

            return true;
        }

        public async Task<bool> TopUpBalanceAsync(Guid userId, decimal amount, string description)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
            {
                await CreateWalletAsync(userId);
                wallet = await _walletRepository.GetByUserIdAsync(userId);
            }

            if (wallet == null) return false;

            wallet.Balance += amount;
            _walletRepository.Save(wallet);

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = WalletTransactionType.TopUp,
                Status = TransactionStatus.Success, // Immediate success for simulated topup
                Description = description,
                CreatedAt = DateTime.UtcNow
            };
            await _walletRepository.AddTransactionAsync(transaction);

            return true;
        }
    }
}
