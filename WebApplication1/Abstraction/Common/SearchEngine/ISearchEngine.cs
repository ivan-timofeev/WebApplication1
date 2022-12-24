namespace WebApplication1.Common.SearchEngine.Abstractions;

public interface ISearchEngine
{
    IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, string searchQuery);
}