using Microsoft.Extensions.Logging;
using Stellar.Shared.Protos;
using Grpc.Net.Client;

namespace CourseService.Infrastructure.Communication.Services;

public interface IUserGrpcClient
{
    Task<UserResponse> GetUserByIdAsync(string userId);
}

public class UserGrpcClient : IUserGrpcClient
{
    private readonly UserLookup.UserLookupClient _client;
    private readonly ILogger<UserGrpcClient> _logger;

    public UserGrpcClient(UserLookup.UserLookupClient client, ILogger<UserGrpcClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<UserResponse> GetUserByIdAsync(string userId)
    {
        try
        {
            _logger.LogInformation("Calling UserService gRPC GetUserById for UserID: {UserId}", userId);
            var request = new GetUserRequest { Id = userId };
            return await _client.GetUserByIdAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling UserService gRPC GetUserById for UserID: {UserId}", userId);
            throw;
        }
    }
}
