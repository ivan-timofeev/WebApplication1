using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions.Data;

namespace WebApplication1.Common.Configuration;

public static class AddDatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:SqlDatabase"];

        if (connectionString == "FROM_ENVIRONMENT")
            throw new Exception("Переменная окружения ConnectionStrings:SqlDatabase не установлена");
        
        Console.WriteLine(connectionString);
        
        services.AddDbContext<WebApplicationDbContext>(
            x => x.UseNpgsql(connectionString),
            contextLifetime: ServiceLifetime.Scoped);

        services.AddScoped<DbContext, WebApplicationDbContext>();

        return services;
    }
}

public interface IConnectionStringProvider
{
    public string GetConnectionString(ConnectionStringTypeEnum connectionStringType);
}

public enum ConnectionStringTypeEnum
{
    SqlDatabase
}
