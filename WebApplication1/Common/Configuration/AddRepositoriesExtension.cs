using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Data.Repositories;

namespace WebApplication1.Common.Configuration;

public static class AddRepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProductsRepository, ProductsRepository>();
        services.AddTransient<IManufacturersRepository, ManufacturersRepository>();
        services.AddTransient<ISalePointsRepository, SalePointsRepository>();
        services.AddTransient<ICustomersRepository, CustomersRepository>();
        services.AddTransient<IOrdersRepository, OrdersRepository>();
        services.AddTransient<ICustomerOrdersRepository, CustomerOrdersRepository>();
        services.AddTransient<IShoppingCartsRepository, ShoppingCartsRepository>();

        return services;
    }
}