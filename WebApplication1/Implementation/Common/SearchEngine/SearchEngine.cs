namespace WebApplication1.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.Abstractions;

public class SearchEngine : ISearchEngine
{
    private readonly ISearchEngineQueryParser _searchEngineQueryParser;

    public SearchEngine(ISearchEngineQueryParser searchEngineQueryParser)
    {
        _searchEngineQueryParser = searchEngineQueryParser;
    }
    
    public IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, string searchQuery)
    {
        var parsedQuery = _searchEngineQueryParser.ParseSearchQuery(searchQuery);

        while (parsedQuery.TryDequeue(out var queryUnit))
        {
            var attributeName = queryUnit.AttributeName;
            var attributeValue = queryUnit.AttributeValue;

            source = queryUnit.Handler.HandleKeyword(source, attributeName, attributeValue);
        }

        return source;
    }
}