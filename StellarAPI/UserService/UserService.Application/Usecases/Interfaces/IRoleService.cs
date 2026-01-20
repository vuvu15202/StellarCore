using Stellar.Shared.Services;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Models.Entities;

namespace UserService.Application.Usecases.Interfaces
{
    public interface IRoleService : IBaseService<Role, Guid, RoleResponse, RoleRequest, RoleResponse>
    {
        Task<MenuResponse> GetPermission(Guid userId);
        Task AssignFunctionGroup(Guid userId, Guid functionGroupId);
    }
}
