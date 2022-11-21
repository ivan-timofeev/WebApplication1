namespace WebApplication1.Helpers.SearchEngine.Abstractions;

public interface ISearchEngine
{
    IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, string searchQuery);
}