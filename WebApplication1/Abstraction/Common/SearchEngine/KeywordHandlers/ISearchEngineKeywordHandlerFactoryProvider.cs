namespace WebApplication1.Common.SearchEngine.KeywordHandlers;

public interface ISearchEngineKeywordHandlerFactoryProvider
{
    ISearchEngineKeywordHandlerFactory GetSearchEngineKeywordHandlerFactory(FilterTypeEnum filterType);
}
