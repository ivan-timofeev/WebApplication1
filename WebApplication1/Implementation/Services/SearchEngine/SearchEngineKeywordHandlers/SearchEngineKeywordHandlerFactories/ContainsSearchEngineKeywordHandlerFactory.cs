
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Abstraction.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class ContainsSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.Contains;

    public ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler()
    {
        return new ContainsSearchEngineKeywordHandler();
    }
}
