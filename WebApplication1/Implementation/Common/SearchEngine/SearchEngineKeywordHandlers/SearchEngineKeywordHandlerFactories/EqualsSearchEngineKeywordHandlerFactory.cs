using WebApplication1.Abstraction.Common.SearchEngine;

namespace WebApplication1.Common.SearchEngine.KeywordHandlers;

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
