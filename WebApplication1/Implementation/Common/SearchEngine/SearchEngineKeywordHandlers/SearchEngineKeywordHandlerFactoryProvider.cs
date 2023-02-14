namespace WebApplication1.Common.SearchEngine.KeywordHandlers;

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
