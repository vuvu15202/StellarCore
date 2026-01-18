using Microsoft.AspNetCore.Http;
using Stellar.Shared.Constants;
using Stellar.Shared.Models;
using System.Threading.Tasks;

namespace Stellar.Shared.Middlewares
{
    public class HeaderContextMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // We can't inject Scoped HeaderContext into Singleton Middleware constructor.
            // But we can resolve it from context.RequestServices (Scoped).
            
            // Actually, best practice in .NET for Scoped services in Middleware is:
            // InvokeAsync(HttpContext context, HeaderContext headerContext)
            // But HeaderContext is a data holder, we need to populate it.
            
            // Strategy:
            // 1. Read header
            // 2. Parse
            // 3. Get HeaderContext from Scoped Services and mutating it? 
            //    Or better, register HeaderContext as Scoped, and in this middleware we get it and set its properties.
            
            // Let's assume HeaderContext is registered as Scoped.
            
            if (context.Request.Headers.TryGetValue(SystemConstant.USER_HEADER, out var userHeader))
            {
                // Retrieve the scoped service
                var headerContext = context.RequestServices.GetService(typeof(HeaderContext)) as HeaderContext;
                if (headerContext != null)
                {
                    // We need a helper to populate it because we designed the class with a constructor.
                    // Let's manually populate or call a helper.
                    var tempContext = new HeaderContext(userHeader.ToString());
                    headerContext.TaiKhoanId = tempContext.TaiKhoanId;
                    headerContext.Ten = tempContext.Ten;
                    headerContext.TaiKhoan = tempContext.TaiKhoan;
                    headerContext.UngDung = tempContext.UngDung;
                    headerContext.DeviceId = tempContext.DeviceId;
                    headerContext.Roles = tempContext.Roles;
                    headerContext.IsAdmin = tempContext.IsAdmin;
                    headerContext.UserId = tempContext.UserId;
                    headerContext.Name = tempContext.Name;
                    headerContext.Username = tempContext.Username;
                    headerContext.ApplicationCode = tempContext.ApplicationCode;
                    headerContext.ExtraData = tempContext.ExtraData;
                    // TraceId might be passed or generated. 
                }
            }

            await _next(context);
        }
    }
}
