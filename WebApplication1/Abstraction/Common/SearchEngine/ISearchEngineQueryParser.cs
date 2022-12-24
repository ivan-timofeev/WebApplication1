using WebApplication1.Common.SearchEngine.Models;

namespace WebApplication1.Common.SearchEngine.Abstractions;

public interface ISearchEngineQueryParser
{
    Queue<SearchEngineQueryUnit> ParseSearchQuery(string searchQuery);
}