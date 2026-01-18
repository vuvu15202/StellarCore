using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Stellar.Shared.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddStellarSwagger(this IServiceCollection services, string title, string version = "v1")
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
            
            c.AddSecurityDefinition("X-User", new OpenApiSecurityScheme
            {
                Name = "X-User",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "JSON Web Token or JSON Object for User Context"
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "X-User" }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }
}
