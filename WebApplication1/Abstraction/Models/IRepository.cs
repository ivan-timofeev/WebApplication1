using WebApplication1.Common.SearchEngine.Models;

namespace WebApplication1.Abstraction.Models;

public interface IRepository<T> : ICrudRepository<T>
    where T : class, IDomainModel
{
    IEnumerable<T> Search(SearchEngineFilter? filter);
    PagedModel<T> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize);
}
