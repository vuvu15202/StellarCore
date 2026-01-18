using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Stellar.Shared.Http;

public static class RefitHelper
{
    public static IHttpClientBuilder RegisterRefitClient<TInterface>(this IServiceCollection services, string baseAddress) 
        where TInterface : class
    {
        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            })
        };

        services.AddTransient<AuthHeaderHandler>();

        return services.AddRefitClient<TInterface>(settings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAddress))
            .AddHttpMessageHandler<AuthHeaderHandler>();
    }
}
