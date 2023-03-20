using WebApplication1.Common.Exceptions;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstractions.Services.SearchEngine;

public interface ISearchEngineFilterValidator
{
    /// <exception cref="SearchEngineFilterValidationException">An exception is thrown if an invalid filter was passed.</exception>
    void ValidateFilter(SearchEngineFilter filter, Type entityType);
}
