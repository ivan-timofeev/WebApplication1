namespace WebApplication1.Common.SearchEngine.Abstractions;

public interface ISearchEngineKeywordHandlerFinder
{
    public ISearchEngineKeywordHandler GetHandler(string keyword);
    public bool IsSupportedOperation(string operation);
}