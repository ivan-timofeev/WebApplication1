namespace WebApplication1.Common.SearchEngine.Abstractions;

public interface ISearchEngineKeywordHandler
{
    string Keyword { get; }
    IQueryable<T> HandleKeyword<T>(IQueryable<T> source, string attributeName, object attributeValue);
}