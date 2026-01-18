using PaymentService.Domain.Models.Entities;
using Stellar.Shared.Interfaces.Persistence;

namespace PaymentService.Domain.Services.Persistence
{
    public interface PaymentPersistence : ICrudPersistence<PaymentTransaction, Guid>
    {
        Task<PaymentTransaction?> GetByOrderIdAsync(string orderId);
        Task<IEnumerable<PaymentTransaction>> GetByUserIdAsync(Guid userId);
    }
}
