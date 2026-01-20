using System;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using Stellar.Shared.Services;
using Stellar.Shared.Interfaces.Persistence;

namespace UserService.Application.Services
{
    public class FunctionGroupService : BaseService<FunctionGroup, Guid, FunctionGroupResponse, FunctionGroupRequest, FunctionGroupResponse>, IFunctionGroupService
    {
        private readonly IFunctionGroupRepository _repository;

        public FunctionGroupService(IFunctionGroupRepository repository)
        {
            _repository = repository;
        }

        public override ICrudPersistence<FunctionGroup, Guid> GetCrudPersistence() => _repository;
    }
}
