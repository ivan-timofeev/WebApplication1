using WebApplication1.Abstractions.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstractions.Services.SearchEngine;

public interface ISearchEngineKeywordHandlerFactoryProvider
{
    ISearchEngineKeywordHandlerFactory GetSearchEngineKeywordHandlerFactory(FilterTypeEnum filterType);
}
