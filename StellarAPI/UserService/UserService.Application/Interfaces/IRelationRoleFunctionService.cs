using System;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;
using Stellar.Shared.Services;

namespace UserService.Application.Interfaces
{
    public interface IRelationRoleFunctionService : IBaseService<RelationRoleFunction, Guid, RelationRoleFunctionResponse, RelationRoleFunctionRequest, RelationRoleFunctionResponse>
    {
    }
}
