using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;
using Stellar.Shared.Services;

namespace UserService.Application.Interfaces
{
    public interface IPlanService : IBaseService<Plan, Guid, PlanResponse, PlanRequest, PlanResponse>
    {
        Task AddPermission(PlanPermissionRequest request);
    }
}
