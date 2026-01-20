using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interfaces;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Domain.Entities;
using Stellar.Shared.APIs;
using Stellar.Shared.Services;
using System;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FunctionGroupsController : BaseApi<FunctionGroup, Guid, FunctionGroupResponse, FunctionGroupRequest, FunctionGroupResponse>
    {
        private readonly IFunctionGroupService _functionGroupService;

        public FunctionGroupsController(IFunctionGroupService functionGroupService)
        {
            _functionGroupService = functionGroupService;
        }

        protected override IBaseService<FunctionGroup, Guid, FunctionGroupResponse, FunctionGroupRequest, FunctionGroupResponse> Service => _functionGroupService;
    }
}
