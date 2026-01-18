using System;
using MediaService.Domain.Models.Entities;
using Stellar.Shared.Interfaces.Persistence;
using MediaService.Domain.Services.Persistence;
using Stellar.Shared.Models;
using Stellar.Shared.Services;
using System.Collections.Generic;
using System.Linq;
using MediaService.Domain.Models.Values;



namespace MediaService.Application.Usecases
{
    public class MediaService : BaseService<Media, Guid, MediaResponse, Media, MediaResponse>
    {
        private readonly MediaPersistence _repository;

        public MediaService(MediaPersistence repository)
        {
            _repository = repository;
        }

        public override ICrudPersistence<Media, Guid> GetCrudPersistence()
        {
            return _repository;
        }
    }
}
