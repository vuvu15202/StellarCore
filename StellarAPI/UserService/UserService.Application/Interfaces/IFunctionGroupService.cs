using System;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;
using Stellar.Shared.Services;

namespace UserService.Application.Interfaces
{
    public interface IFunctionGroupService : IBaseService<FunctionGroup, Guid, FunctionGroupResponse, FunctionGroupRequest, FunctionGroupResponse>
    {
    }
}
