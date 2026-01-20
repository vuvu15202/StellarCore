using System;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using Stellar.Shared.Services;
using UserService.Domain.Models.Entities;

namespace UserService.Application.Usecases.Interfaces
{
    public interface IFunctionGroupService : IBaseService<FunctionGroup, Guid, FunctionGroupResponse, FunctionGroupRequest, FunctionGroupResponse>
    {
    }
}
