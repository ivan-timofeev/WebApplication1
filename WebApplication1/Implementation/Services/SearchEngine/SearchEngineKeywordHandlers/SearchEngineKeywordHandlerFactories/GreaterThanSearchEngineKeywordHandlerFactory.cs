
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Abstraction.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class GreaterThanSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.GreaterThan;
    
    private readonly ISearchEngineFilterAttributeParser _searchEngineFilterAttributeParser;

    public GreaterThanSearchEngineKeywordHandlerFactory(ISearchEngineFilterAttributeParser searchEngineFilterAttributeParser)
    {
        _searchEngineFilterAttributeParser = searchEngineFilterAttributeParser;
    }

    public ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler()
    {
        return new GreaterThanSearchEngineKeywordHandler(_searchEngineFilterAttributeParser);
    }
}
