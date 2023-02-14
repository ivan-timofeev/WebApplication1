
using WebApplication1.Abstraction.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstraction.Services.SearchEngine;

public interface ISearchEngineKeywordHandlerFactoryProvider
{
    ISearchEngineKeywordHandlerFactory GetSearchEngineKeywordHandlerFactory(FilterTypeEnum filterType);
}
