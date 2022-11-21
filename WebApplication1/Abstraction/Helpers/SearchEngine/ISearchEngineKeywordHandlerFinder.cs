namespace WebApplication1.Helpers.SearchEngine.Abstractions;

public interface ISearchEngineKeywordHandlerFinder
{
    public ISearchEngineKeywordHandler GetHandler(string keyword);
    public bool IsSupportedOperation(string operation);
}