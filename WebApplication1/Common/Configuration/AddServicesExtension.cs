using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Services;
using WebApplication1.Common.Extensions;
using WebApplication1.Data.Repositories;
using WebApplication1.Services;
using WebApplication1.Services.SearchEngine.DI;

namespace WebApplication1.Common.Configuration;

public static class AddAddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddLogging(x => x.AddConsole());
        services.AddConfiguredAutoMapper();
        services.AddSearchEngine();

        services.AddTransient<IProductsManagementService, ProductsManagementService>();
        services.AddTransient<ISalePointsManagementService, SalePointsManagementService>();
        services.AddTransient<ICustomersManagementService, CustomersManagementService>();
        
        services.AddOrdersManagementService();
        services.AddScoped<IDatabaseTransactionsManagementService, DatabaseTransactionsManagementService>();
        services.AddTransient<IFilesManagementService, FilesManagementService>();
        services.AddTransient<IShoppingCartsManagementService, ShoppingCartsManagementService>();
        services.AddTransient<ISaleItemsRepository, SaleItemsRepository>();

        return services;
    }
}
