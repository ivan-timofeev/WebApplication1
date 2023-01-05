using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Services;
using WebApplication1.Common.SearchEngine.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Services;

namespace WebApplication1.Implementation.Helpers.Configuration;

public static class AddAddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddLogging(x => x.AddConsole());
        services.AddConfiguredAutoMapper();
        services.AddSearchEngine();

        services.AddTransient<IOrdersManagementService, OrdersManagementService>();

        return services;
    }
}
