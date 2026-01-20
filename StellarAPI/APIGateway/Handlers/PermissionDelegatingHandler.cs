
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace APIGateway.Handlers
{
    public class PermissionDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                return Forbidden("Unauthenticated");
            }

            var permissions = user.Claims
                .Where(c => c.Type == "permissions")
                .Select(c => c.Value)
                .ToHashSet();

            var requiredPermission =
                request.Headers.TryGetValues("RequiredPermission", out var values)
                    ? values.First()
                    : null;

            if (!string.IsNullOrEmpty(requiredPermission)
                && !permissions.Contains(requiredPermission))
            {
                return Forbidden($"Missing permission: {requiredPermission}");
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private HttpResponseMessage Forbidden(string message)
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(JsonSerializer.Serialize(new
                {
                    message
                }))
            };
        }
    }


}
