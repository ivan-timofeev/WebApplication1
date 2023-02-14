
using WebApplication1.Common.SearchEngine.Models;

namespace WebApplication1.Abstraction.Common.SearchEngine;

public interface ISearchEngine
{
    public IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, SearchEngineFilter filter);
}
