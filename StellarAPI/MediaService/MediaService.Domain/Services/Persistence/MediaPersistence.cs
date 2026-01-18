using Stellar.Shared.Interfaces.Persistence;
using MediaService.Domain.Models.Entities;
using System;

namespace MediaService.Domain.Services.Persistence;

public interface MediaPersistence : IBasePersistence<Media, Guid>
{
}
