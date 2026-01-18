using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Services.Persistence;
using PaymentService.Infrastructure.Database;
using Stellar.Shared.Repositories;

namespace PaymentService.Infrastructure.Services.Repository
{
    public class WalletRepository : CrudRepository<Wallet, Guid>, WalletPersistence
    {
        private readonly PaymentDbContext _context;

        public WalletRepository(PaymentDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<WalletTransaction> AddTransactionAsync(WalletTransaction transaction)
        {
            await _context.WalletTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsByWalletIdAsync(Guid walletId)
        {
            return await _context.WalletTransactions
                .Where(wt => wt.WalletId == walletId)
                .OrderByDescending(wt => wt.CreatedAt)
                .ToListAsync();
        }
    }
}
