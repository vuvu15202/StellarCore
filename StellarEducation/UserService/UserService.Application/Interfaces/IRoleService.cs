using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IRoleService
    {
        Task AddPermission(PermissionRequest request);
        Task<MenuResponse> GetPermission(Guid userId);
    }
}
