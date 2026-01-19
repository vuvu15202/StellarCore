using Microsoft.EntityFrameworkCore;
using MediaService.Domain.Models.Entities;

namespace MediaService.Infrastructure.Data;

public class MediaDbContext : DbContext
{
    public MediaDbContext(DbContextOptions<MediaDbContext> options) : base(options)
    {
    }

    public DbSet<Media> Medias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Media>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.StoragePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Hierarchy).HasMaxLength(1000);
            entity.Property(e => e.Path).HasMaxLength(500); // Keeping for legacy or virtual path
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            
            entity.HasIndex(e => e.ParentId);
        });
    }
}
