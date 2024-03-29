using WebApplication1.Abstractions.Services;
using WebApplication1.Services.OrdersManagementService;

namespace WebApplication1.Common.Extensions;

public static class AddOrdersManagementServiceExtension
{
    public static IServiceCollection AddOrdersManagementService(this IServiceCollection services)
    {
        services
            .AddUpdateOrderStateStrategies()
            .AddTransient<IUpdateOrderStateStrategyResolver, UpdateOrderStateStrategyResolver>()
            .AddTransient<IOrdersManagementService, OrdersManagementService>();

        return services;
    }

    private static IServiceCollection AddUpdateOrderStateStrategies(this IServiceCollection services)
    {
        var type = typeof(IUpdateOrderStateStrategy);
        var inheritances = GetAllInterfaceInheritances(type);

        foreach (var inheritance in inheritances)
        {
            services.AddTransient(type, inheritance);
        }

        return services;
    }

    private static IEnumerable<Type> GetAllInterfaceInheritances(Type type)
    {
        var result = type.Assembly
            .GetTypes()
            .Where(p => type.IsAssignableFrom(p) && p != type);

        return result;
    }
}
