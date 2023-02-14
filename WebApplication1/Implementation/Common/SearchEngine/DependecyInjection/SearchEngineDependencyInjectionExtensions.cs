using WebApplication1.Abstraction.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.KeywordHandlers;

namespace WebApplication1.Common.SearchEngine.DependencyInjection;

public static class SearchEngineDependencyInjectionExtensions
{
    public static IServiceCollection AddSearchEngine(this IServiceCollection services)
    {
        services
            .BindSearchEngineKeywordHandlers()
            .AddSearchEngineKeywordHandlerFactories()
            .AddAttributeParserStrategies()
            .AddTransient<ISearchEngineFilterAttributeParser, SearchEngineFilterAttributeParser>()
            .AddTransient<ISearchEngineKeywordHandlerFactoryProvider, SearchEngineKeywordHandlerFactoryProvider>()
            .AddTransient<ISearchEngineFilterValidator, SearchEngineFilterValidator>()
            .AddTransient<ISearchEngine, SearchEngine>();

        return services;
    }

    private static IServiceCollection AddSearchEngineKeywordHandlerFactories(this IServiceCollection services)
    {
        var type = typeof(ISearchEngineKeywordHandlerFactory);
        var types = type.Assembly
            .GetTypes()
            .Where(p => type.IsAssignableFrom(p) && p != type);

        foreach (var inheritance in types)
        {
            services.AddTransient(type, inheritance);
        }

        return services;
    }

    private static IServiceCollection BindSearchEngineKeywordHandlers(this IServiceCollection services)
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

    private static IServiceCollection AddAttributeParserStrategies(this IServiceCollection services)
    {
        var type = typeof(IAttributeParseStrategy);
        var types = type.Assembly
            .GetTypes()
            .Where(p => type.IsAssignableFrom(p) && p != type);

        foreach (var inheritance in types)
        {
            services.AddTransient(type, inheritance);
        }

        return services;
    }
}
