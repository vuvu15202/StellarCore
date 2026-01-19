using Stellar.Shared.Interfaces.Persistence;
using MediaService.Domain.Models.Entities;
using System;

namespace MediaService.Domain.Services.Persistence;

public interface MediaPersistence : IBasePersistence<Media, Guid>
{
    Task<List<Media>> FindAllByParentIdAsync(Guid? parentId);
    Task<Media?> FindByNameAndParentIdAsync(string name, Guid? parentId);
    Task<Media?> FindByIdAsync(Guid id);
    Task<Media> SaveAsync(Media media);
}
