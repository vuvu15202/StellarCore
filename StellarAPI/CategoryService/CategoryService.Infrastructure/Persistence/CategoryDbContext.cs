using Microsoft.EntityFrameworkCore;
using CategoryService.Domain.Entities;

namespace CategoryService.Infrastructure.Persistence
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options) { }

        public DbSet<CategoryType> CategoryTypes { get; set; } = null!;
        public DbSet<CommonCategory> CommonCategories { get; set; } = null!;
        public DbSet<BloodTypeCategory> BloodTypeCategories { get; set; } = null!;
        public DbSet<DistrictCategory> DistrictCategories { get; set; } = null!;
        public DbSet<EthnicCategory> EthnicCategories { get; set; } = null!;
        public DbSet<FaqCategory> FaqCategories { get; set; } = null!;
        public DbSet<MaritalStatusCategory> MaritalStatusCategories { get; set; } = null!;
        public DbSet<NationCategory> NationCategories { get; set; } = null!;
        public DbSet<ProvinceCategory> ProvinceCategories { get; set; } = null!;
        public DbSet<RelationShipCategory> RelationShipCategories { get; set; } = null!;
        public DbSet<ReligionCategory> ReligionCategories { get; set; } = null!;
        public DbSet<SexualCategory> SexualCategories { get; set; } = null!;
        public DbSet<WardCategory> WardCategories { get; set; } = null!;
        public DbSet<UserManualCategory> UserManualCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CategoryType>(entity =>
            {
                entity.HasIndex(e => e.Code).IsUnique();
            });

            // Configure BaseCategory hierarchy if needed, 
            // but we are using [Table] attributes for TPT or TPC.
            // EF Core will use the [Table] attributes to map to separate tables.
        }
    }
}
