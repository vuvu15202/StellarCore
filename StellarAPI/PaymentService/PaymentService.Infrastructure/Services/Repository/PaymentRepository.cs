using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Models.Entities;
using PaymentService.Domain.Services.Persistence;
using PaymentService.Infrastructure.Database;
using Stellar.Shared.Repositories;

namespace PaymentService.Infrastructure.Services.Repository
{
    public class PaymentRepository : CrudRepository<PaymentTransaction, Guid>, PaymentPersistence
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PaymentTransaction?> GetByOrderIdAsync(string orderId)
        {
            return await _context.PaymentTransactions
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<PaymentTransaction>> GetByUserIdAsync(Guid userId)
        {
            return await _context.PaymentTransactions
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
