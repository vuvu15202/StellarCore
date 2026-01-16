using Microsoft.EntityFrameworkCore;
using CourseService.Domain.Entities;

namespace CourseService.Infrastructure.Database
{
    public class StellarDbContext : DbContext
    {
        public StellarDbContext(DbContextOptions<StellarDbContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");
                entity.HasKey(e => e.Id);
                // Configure other properties if needed
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}
