using Microsoft.Extensions.DependencyInjection;
using Stellar.Shared.Excel;

namespace Stellar.Shared.Extensions;

public static class ExcelExtensions
{
    public static IServiceCollection AddStellarExcel(this IServiceCollection services)
    {
        services.AddScoped<IExcelService, ExcelService>();
        return services;
    }
}
