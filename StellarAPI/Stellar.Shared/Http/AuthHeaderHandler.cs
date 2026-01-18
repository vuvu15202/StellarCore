using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Stellar.Shared.Http;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            // Propagate Authorization header
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeader);
            }

            // Propagate custom X-User header if exists
            if (context.Request.Headers.TryGetValue("X-User", out StringValues userHeader))
            {
                request.Headers.TryAddWithoutValidation("X-User", userHeader.ToString());
            }
            
            // Propagate Trace-Id if exists
            if (context.Request.Headers.TryGetValue("X-Trace-Id", out StringValues traceId))
            {
                request.Headers.TryAddWithoutValidation("X-Trace-Id", traceId.ToString());
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
