using WebApplication1.Abstraction.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.KeywordHandlers;

namespace WebApplication1.Common.SearchEngine.KeywordHandlers;

public class ContainsSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.Contains;

    public ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler()
    {
        return new ContainsSearchEngineKeywordHandler();
    }
}
