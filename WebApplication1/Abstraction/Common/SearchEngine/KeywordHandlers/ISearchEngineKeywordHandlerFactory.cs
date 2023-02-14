using WebApplication1.Abstraction.Common.SearchEngine;

namespace WebApplication1.Common.SearchEngine.KeywordHandlers;

public interface ISearchEngineKeywordHandlerFactory
{
    FilterTypeEnum FilterType { get; }
    ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler();
}
