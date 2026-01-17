using MediaService.Domain.Entities;
using Stellar.Shared.Interfaces.Persistence;
using System;

namespace MediaService.Application.Interfaces;

public interface IMediaRepository : IBasePersistence<Media, Guid>
{
}
