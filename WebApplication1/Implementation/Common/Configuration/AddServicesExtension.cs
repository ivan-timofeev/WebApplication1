using Microsoft.EntityFrameworkCore;
using WebApplication1.Common.SearchEngine.DependencyInjection;
using WebApplication1.Data;

namespace WebApplication1.Implementation.Helpers.Configuration;

public static class AddAddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddLogging(x => x.AddConsole());
        services.AddConfiguredAutoMapper();
        services.AddSearchEngine();
        
        return services;
    }
}