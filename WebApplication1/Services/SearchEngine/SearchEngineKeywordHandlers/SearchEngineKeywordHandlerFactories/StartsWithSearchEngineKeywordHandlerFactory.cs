using WebApplication1.Abstractions.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class StartsWithSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.StartsWith;

    public ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler()
    {
        return new StartsWithSearchEngineKeywordHandler();
    }
}
