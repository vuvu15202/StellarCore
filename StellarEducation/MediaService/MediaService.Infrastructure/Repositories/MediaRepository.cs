using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using MediaService.Infrastructure.Data;
using Stellar.Shared.Repositories;
using System;

namespace MediaService.Infrastructure.Repositories;

public class MediaRepository : CrudRepository<Media, Guid>, IMediaRepository
{
    public MediaRepository(MediaDbContext context) : base(context)
    {
    }
}
