using Microsoft.AspNetCore.Mvc;
using CourseService.Infrastructure.Services.Communication;

namespace CourseService.API.APIs;

[ApiController]
[Route("api/[controller]")]
public class TestGrpcController : ControllerBase
{
    private readonly IUserGrpcClient _userGrpcClient;
    private readonly ILogger<TestGrpcController> _logger;

    public TestGrpcController(IUserGrpcClient userGrpcClient, ILogger<TestGrpcController> logger)
    {
        _userGrpcClient = userGrpcClient;
        _logger = logger;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUser(string userId)
    {
        try
        {
            _logger.LogInformation("Testing gRPC: Getting user {UserId}", userId);
            var user = await _userGrpcClient.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC Test failed for user {UserId}", userId);
            return StatusCode(500, new { error = "gRPC call failed", message = ex.Message });
        }
    }
}
