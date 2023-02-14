using WebApplication1.Abstraction.Services;
using WebApplication1.Services;
using WebApplication1.Services.SearchEngine.DI;

namespace WebApplication1.Implementation.Helpers.Configuration;

public static class AddAddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddLogging(x => x.AddConsole());
        services.AddConfiguredAutoMapper();
        services.AddSearchEngine();

        services.AddTransient<IOrdersManagementService, OrdersManagementService>();
        services.AddScoped<IDatabaseTransactionsManagementService, DatabaseTransactionsManagementService>();

        return services;
    }
}
