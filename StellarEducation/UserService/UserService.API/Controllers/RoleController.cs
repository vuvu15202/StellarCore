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

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("add-permission")]
        public async Task<IActionResult> AddPermission([FromBody] PermissionRequest request)
        {
            try
            {
                await _roleService.AddPermission(request);
                return Ok(ApiResponse<string>.SuccessResponse("Permissions updated successfully."));
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error updating permissions.", new System.Collections.Generic.List<string> { ex.Message }));
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
