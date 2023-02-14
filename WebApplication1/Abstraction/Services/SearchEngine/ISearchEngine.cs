
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstraction.Services.SearchEngine;

public interface ISearchEngine
{
    public IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, SearchEngineFilter? filter);
}
