using Microsoft.EntityFrameworkCore;
using ExaminationService.Domain.Models.Entities;

namespace ExaminationService.Infrastructure.Database
{
    public class StellarDbContext : DbContext
    {
        public StellarDbContext(DbContextOptions<StellarDbContext> options) : base(options)
        {
        }

        public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExamAttempt>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.StateType)
                      .HasConversion<int>()
                      .IsRequired();
            });
        }
    }
}
