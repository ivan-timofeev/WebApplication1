using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Implementation.Helpers.Configuration;

public static class AddDatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var connectionString = @"Server=localhost;Database=my_db;Username=postgres;Password=123456;";
        
        services.AddDbContext<WebApplicationDbContext>(
            x => x.UseNpgsql(connectionString),
            contextLifetime: ServiceLifetime.Scoped);

        return services;
    }
}