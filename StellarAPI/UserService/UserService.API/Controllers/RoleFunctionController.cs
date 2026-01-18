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
    public class RoleFunctionController : BaseApi<RelationRoleFunction, Guid, RelationRoleFunctionResponse, RelationRoleFunctionRequest, RelationRoleFunctionResponse>
    {
        private readonly IRelationRoleFunctionService _roleFunctionService;

        public RoleFunctionController(IRelationRoleFunctionService roleFunctionService)
        {
            _roleFunctionService = roleFunctionService;
        }

        protected override IBaseService<RelationRoleFunction, Guid, RelationRoleFunctionResponse, RelationRoleFunctionRequest, RelationRoleFunctionResponse> Service => _roleFunctionService;
    }
}
