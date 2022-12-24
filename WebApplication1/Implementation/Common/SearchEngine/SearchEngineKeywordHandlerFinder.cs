namespace WebApplication1.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.Abstractions;

public class SearchEngineKeywordHandlerFinder : ISearchEngineKeywordHandlerFinder
{
    private readonly IEnumerable<ISearchEngineKeywordHandler> _registeredSearchKeywordHandler;
    
    public SearchEngineKeywordHandlerFinder(IEnumerable<ISearchEngineKeywordHandler> registeredSearchKeywordHandler)
    {
        _registeredSearchKeywordHandler = registeredSearchKeywordHandler;
    }
    
    public ISearchEngineKeywordHandler GetHandler(string keyword)
    {
        return _registeredSearchKeywordHandler.Single(x =>
            string.Equals(x.Keyword, keyword, StringComparison.InvariantCultureIgnoreCase));
    }

    public bool IsSupportedOperation(string keyword)
    {
        return _registeredSearchKeywordHandler.Any(x =>
            string.Equals(x.Keyword, keyword, StringComparison.InvariantCultureIgnoreCase));
    }
}