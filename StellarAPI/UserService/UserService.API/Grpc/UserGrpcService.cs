using Grpc.Core;
using Stellar.Shared.Protos;
using UserService.Application.Usecases.Interfaces;

namespace UserService.API.Grpc;

public class UserGrpcService : UserLookup.UserLookupBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserGrpcService> _logger;

    public UserGrpcService(IUserService userService, ILogger<UserGrpcService> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public override async Task<UserResponse> GetUserById(GetUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC GetUserById called for UserID: {UserId}", request.Id);

        if (!Guid.TryParse(request.Id, out var userId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid User ID format"));
        }

        var profile = await _userService.GetProfileAsync(userId);

        if (profile == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"User with ID {request.Id} not found"));
        }

        return new UserResponse
        {
            Id = profile.UserId.ToString(),
            Username = profile.UserName ?? "",
            FullName = profile.FullName ?? "",
            Email = profile.Email ?? "",
            AvatarUrl = profile.ProfilePhotoUrl ?? ""
        };
    }
}
