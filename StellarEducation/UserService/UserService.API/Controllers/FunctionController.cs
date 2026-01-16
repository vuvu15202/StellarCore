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
    public class FunctionController : BaseApi<Function, Guid, FunctionResponse, FunctionRequest, FunctionResponse>
    {
        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        protected override IBaseService<Function, Guid, FunctionResponse, FunctionRequest, FunctionResponse> Service => _functionService;
    }
}
