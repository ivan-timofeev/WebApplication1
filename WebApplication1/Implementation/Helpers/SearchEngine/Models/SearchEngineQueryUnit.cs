using WebApplication1.Helpers.SearchEngine.Abstractions;

namespace WebApplication1.Helpers.SearchEngine.Models;

public class SearchEngineQueryUnit
{
    public ISearchEngineKeywordHandler Handler { get; }
    public string AttributeName { get; }
    public object? AttributeValue { get; }

    public SearchEngineQueryUnit(ISearchEngineKeywordHandler handler, string attributeName, object? attributeValue)
    {
        AttributeValue = attributeValue;
        AttributeName = attributeName;
        Handler = handler;
    }
}