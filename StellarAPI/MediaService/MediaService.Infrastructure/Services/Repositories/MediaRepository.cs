using MediaService.Application.Interfaces;
using MediaService.Domain.Models.Entities;
using MediaService.Infrastructure.Data;
using Stellar.Shared.Repositories;
using MediaService.Domain.Services.Persistence;

using System;

namespace MediaService.Infrastructure.Services.Repositories;

public class MediaRepository : CrudRepository<Media, Guid>, MediaPersistence
{
    public MediaRepository(MediaDbContext context) : base(context)
    {
    }
}
