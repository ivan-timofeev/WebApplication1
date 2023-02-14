
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstraction.Services.SearchEngine.KeywordHandlers;

public interface ISearchEngineKeywordHandlerFactory
{
    FilterTypeEnum FilterType { get; }
    ISearchEngineKeywordHandler CreateSearchEngineKeywordHandler();
}
