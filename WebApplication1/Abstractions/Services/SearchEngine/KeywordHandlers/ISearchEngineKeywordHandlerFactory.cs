using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstractions.Services.SearchEngine.KeywordHandlers;

public interface ISearchEngineKeywordHandlerFactory
{
    FilterTypeEnum FilterType { get; }
    ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler();
}
