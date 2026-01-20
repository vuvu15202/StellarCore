using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Application.DTOs.Requests;
using UserService.API.Controllers;
using UserService.API.DTOs;
using Stellar.Shared.Models;
using UserService.Application.DTOs.Responses;
using System.Collections.Generic;
using System;
using Stellar.Shared.APIs;
using Stellar.Shared.Services;
using UserService.Infrastructure.Identity;
using UserService.Domain.Entities;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : BaseApi<Role, Guid, RoleResponse, RoleRequest, RoleResponse>
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override IBaseService<Role, Guid, RoleResponse, RoleRequest, RoleResponse> Service => _roleService;

        [HttpPost("assign-group")]
        public async Task<IActionResult> AssignFunctionGroup([FromBody] AssignFunctionGroupRequest request)
        {
            try
            {
                await _roleService.AssignFunctionGroup(request.UserId, request.FunctionGroupId);
                return Ok(ApiResponse<string>.SuccessResponse("Function group assigned successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error assigning function group.", new List<string> { ex.Message }));
            }
        }

        [HttpGet("permissions/{userId}")]
        public async Task<IActionResult> GetPermissions(Guid userId)
        {
            try
            {
                var permissions = await _roleService.GetPermission(userId);
                return Ok(ApiResponse<MenuResponse>.SuccessResponse(permissions ?? new MenuResponse()));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error retrieving permissions.", new System.Collections.Generic.List<string> { ex.Message }));
            }
        }
    }
}
