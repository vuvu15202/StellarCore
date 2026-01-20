using System;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using Stellar.Shared.Services;
using Stellar.Shared.Interfaces.Persistence;
using UserService.Domain.Models.Entities;
using UserService.Domain.Services.Persistence;
using UserService.Application.Usecases.Interfaces;

namespace UserService.Application.Usecases
{
    public class FunctionGroupService : BaseService<FunctionGroup, Guid, FunctionGroupResponse, FunctionGroupRequest, FunctionGroupResponse>, IFunctionGroupService
    {
        private readonly FunctionGroupPersistence _repository;

        public FunctionGroupService(FunctionGroupPersistence repository)
        {
            _repository = repository;
        }

        public override ICrudPersistence<FunctionGroup, Guid> GetCrudPersistence() => _repository;
    }
}
