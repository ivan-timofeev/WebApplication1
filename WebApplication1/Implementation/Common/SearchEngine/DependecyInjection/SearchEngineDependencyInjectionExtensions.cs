using WebApplication1.Common.SearchEngine.Abstractions;
namespace WebApplication1.Common.SearchEngine.DependencyInjection;

public static class SearchEngineDependencyInjectionExtensions
{
    private static IServiceCollection AddSearchEngineKeywordHandlers(this IServiceCollection services)
    {
        var type = typeof(ISearchEngineKeywordHandler);
        var types = type.Assembly
            .GetTypes()
            .Where(p => type.IsAssignableFrom(p) && p != type);

        foreach (var inheritance in types)
        {
            services.AddTransient(type, inheritance);
        }

        return services;
    }

    public static IServiceCollection AddSearchEngine(this IServiceCollection services)
    {
        services
            .AddSearchEngineKeywordHandlers()
            .AddTransient<ISearchEngineKeywordHandlerFinder, SearchEngineKeywordHandlerFinder>()
            .AddTransient<ISearchEngineQueryParser, SearchEngineQueryParser>()
            .AddTransient<ISearchEngine, SearchEngine>();

        return services;
    }
}