using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using Stellar.Shared.APIs;
using Stellar.Shared.Services;
using System;
using System.Threading.Tasks;
using UserService.Domain.Models.Entities;
using UserService.Application.Usecases.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : BaseApi<Plan, Guid, PlanResponse, PlanRequest, PlanResponse>
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        protected override IBaseService<Plan, Guid, PlanResponse, PlanRequest, PlanResponse> Service => _planService;

        [HttpPost("permissions")]
        public async Task<IActionResult> AddPermission([FromBody] PlanPermissionRequest request)
        {
            await _planService.AddPermission(request);
            return Ok(new { message = "Permissions added to plan successfully" });
        }
    }
}
