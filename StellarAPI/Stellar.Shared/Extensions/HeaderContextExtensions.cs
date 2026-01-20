using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Stellar.Shared.Middlewares;
using Stellar.Shared.Models;

namespace Stellar.Shared.Extensions
{
    public static class HeaderContextExtensions
    {
        public static IServiceCollection AddHeaderContext(this IServiceCollection services)
        {
            services.AddScoped<HeaderContext>();
            return services;
        }

        public static IApplicationBuilder UseHeaderContext(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HeaderContextMiddleware>();
        }
    }
}
