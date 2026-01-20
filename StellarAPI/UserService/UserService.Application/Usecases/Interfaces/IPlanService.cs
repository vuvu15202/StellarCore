using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using Stellar.Shared.Services;
using UserService.Domain.Models.Entities;

namespace UserService.Application.Usecases.Interfaces
{
    public interface IPlanService : IBaseService<Plan, Guid, PlanResponse, PlanRequest, PlanResponse>
    {
        Task AddPermission(PlanPermissionRequest request);
    }
}
