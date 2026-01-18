using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Models.Entities;

namespace PaymentService.Infrastructure.Database
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
        public DbSet<Wallet> Wallets { get; set; } = null!;
        public DbSet<WalletTransaction> WalletTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Decimal precision configuration
            modelBuilder.Entity<PaymentTransaction>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<WalletTransaction>()
                .Property(wt => wt.Amount)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Wallet>()
                .HasIndex(w => w.UserId)
                .IsUnique();
        }
    }
}
