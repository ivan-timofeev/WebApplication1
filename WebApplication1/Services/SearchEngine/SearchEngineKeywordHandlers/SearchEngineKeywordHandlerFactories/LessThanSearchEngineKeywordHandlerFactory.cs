using WebApplication1.Abstractions.Services.SearchEngine;
using WebApplication1.Abstractions.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class LessThanSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.LessThan;
    
    private readonly ISearchEngineFilterAttributeParser _searchEngineFilterAttributeParser;

    public LessThanSearchEngineKeywordHandlerFactory(ISearchEngineFilterAttributeParser searchEngineFilterAttributeParser)
    {
        _searchEngineFilterAttributeParser = searchEngineFilterAttributeParser;
    }

    public ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler()
    {
        return new LessThanSearchEngineKeywordHandler(_searchEngineFilterAttributeParser);
    }
}
