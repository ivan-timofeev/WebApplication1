
using WebApplication1.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.Models;

namespace WebApplication1.Abstraction.Common.SearchEngine;

public interface ISearchEngineFilterValidator
{
    /// <exception cref="SearchEngineFilterValidationException">An exception is thrown if an invalid filter was passed.</exception>
    void ValidateFilter(SearchEngineFilter filter, Type entityType);
}
