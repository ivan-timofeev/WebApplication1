using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Abstraction.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine;
public class SearchEngineKeywordHandlerFactoryProvider
    : ISearchEngineKeywordHandlerFactoryProvider
{
    private readonly ISearchEngineKeywordHandlerFactory[] _searchEngineKeywordHandlerFactories;

    public SearchEngineKeywordHandlerFactoryProvider(
        IEnumerable<ISearchEngineKeywordHandlerFactory> searchEngineKeywordHandlerFactories)
    {
        _searchEngineKeywordHandlerFactories = searchEngineKeywordHandlerFactories.ToArray();
    }

    public ISearchEngineKeywordHandlerFactory GetSearchEngineKeywordHandlerFactory(FilterTypeEnum filterType)
    {
        return _searchEngineKeywordHandlerFactories.Single(x => x.FilterType == filterType);
    }
}
