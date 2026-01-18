using PaymentService.Domain.Models.Entities;
using Stellar.Shared.Interfaces.Persistence;

namespace PaymentService.Domain.Services.Persistence
{
    public interface WalletPersistence : ICrudPersistence<Wallet, Guid>
    {
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task<WalletTransaction> AddTransactionAsync(WalletTransaction transaction);
        Task<IEnumerable<WalletTransaction>> GetTransactionsByWalletIdAsync(Guid walletId);
    }
}
