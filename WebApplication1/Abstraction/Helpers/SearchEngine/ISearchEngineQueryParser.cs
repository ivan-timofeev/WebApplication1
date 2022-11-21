using WebApplication1.Helpers.SearchEngine.Models;

namespace WebApplication1.Helpers.SearchEngine.Abstractions;

public interface ISearchEngineQueryParser
{
    Queue<SearchEngineQueryUnit> ParseSearchQuery(string searchQuery);
}