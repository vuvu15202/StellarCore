using MediaService.Domain.Models.Entities;
using MediaService.Infrastructure.Data;
using Stellar.Shared.Repositories;
using MediaService.Domain.Services.Persistence;

using System;
using System.Collections.Generic; // For List
using System.Linq; // For Where
using System.Threading.Tasks; // For Task
using Microsoft.EntityFrameworkCore; // For ToListAsync, FirstOrDefaultAsync

namespace MediaService.Infrastructure.Services.Repositories;

public class MediaRepository : CrudRepository<Media, Guid>, MediaPersistence
{
    private readonly MediaDbContext _context;

    public MediaRepository(MediaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Media>> FindAllByParentIdAsync(Guid? parentId)
    {
        return await _context.Set<Media>()
            .Where(m => m.ParentId == parentId && !m.IsDeleted) // Assuming IsDeleted from AuditingEntity/SoftDelete
            .ToListAsync();
    }

    public async Task<Media?> FindByNameAndParentIdAsync(string name, Guid? parentId)
    {
        return await _context.Set<Media>()
            .FirstOrDefaultAsync(m => m.Name == name && m.ParentId == parentId && !m.IsDeleted);
    }

    public async Task<Media?> FindByIdAsync(Guid id)
    {
        return await _context.Set<Media>().FindAsync(id);
    }

    public async Task<Media> SaveAsync(Media media)
    {
        // Check if entity is already tracked or exists
        var existing = await _context.Set<Media>().FindAsync(media.Id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(media);
        }
        else
        {
            await _context.Set<Media>().AddAsync(media);
        }
        await _context.SaveChangesAsync();
        return media;
    }
}
