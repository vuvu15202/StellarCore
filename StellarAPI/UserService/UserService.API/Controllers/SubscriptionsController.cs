using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs.Requests;
using System;
using System.Threading.Tasks;
using UserService.Application.Usecases.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionRequest request)
        {
            var result = await _subscriptionService.Subscribe(request);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetActiveSubscription(Guid userId)
        {
            var result = await _subscriptionService.GetActiveSubscription(userId);
            if (result == null) return NotFound(new { message = "No active subscription found" });
            return Ok(result);
        }
    }
}
