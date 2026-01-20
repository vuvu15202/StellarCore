using Stellar.Shared.Services;
using UserService.Domain.Entities;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Interfaces
{
    public interface IRoleService : IBaseService<Role, Guid, RoleResponse, RoleRequest, RoleResponse>
    {
        Task<MenuResponse> GetPermission(Guid userId);
        Task AssignFunctionGroup(Guid userId, Guid functionGroupId);
    }
}
