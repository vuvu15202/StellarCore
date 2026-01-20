using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using Stellar.Shared.APIs;
using Stellar.Shared.Services;
using System;
using UserService.Domain.Models.Entities;
using UserService.Application.Usecases.Interfaces;

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
