using System.Security.Claims;
using System.Text.Json;
using Stellar.Shared.Constants;

namespace APIGateway.Middlewares
{
    public class HeaderContextMappingMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderContextMappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userClaims = new Dictionary<string, object>();
                
                // Map common identity claims to HeaderContext fields
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? context.User.FindFirst("sub")?.Value;
                var userName = context.User.FindFirst(ClaimTypes.Name)?.Value 
                            ?? context.User.FindFirst("unique_name")?.Value;
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value;

                if (!string.IsNullOrEmpty(userId)) userClaims["taiKhoanId"] = userId;
                if (!string.IsNullOrEmpty(userName)) userClaims["taiKhoan"] = userName;
                if (!string.IsNullOrEmpty(userName)) userClaims["ten"] = userName; // Default to username if full name not available
                
                // Collect roles
                var roles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                if (roles.Any())
                {
                    userClaims["roles"] = roles;
                }

                // Serialize to JSON and add to X-User header
                var xUserJson = JsonSerializer.Serialize(userClaims);
                context.Request.Headers[SystemConstant.USER_HEADER] = xUserJson;
            }

            await _next(context);
        }
    }

    public static class HeaderContextMappingExtensions
    {
        public static IApplicationBuilder UseHeaderContextMapping(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HeaderContextMappingMiddleware>();
        }
    }
}
