
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Abstraction.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class EqualsSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.Equals;
    
    private readonly ISearchEngineFilterAttributeParser _searchEngineFilterAttributeParser;

    public EqualsSearchEngineKeywordHandlerFactory(ISearchEngineFilterAttributeParser searchEngineFilterAttributeParser)
    {
        _searchEngineFilterAttributeParser = searchEngineFilterAttributeParser;
    }

    public ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler()
    {
        return new EqualsSearchEngineKeywordHandler(_searchEngineFilterAttributeParser);
    }
}
