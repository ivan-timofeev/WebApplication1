namespace WebApplication1.Abstraction.Models;

public interface IRepository<T> : ICrudRepository<T>
    where T : class, IDomainModel
{
    IEnumerable<T> Search(string? searchQuery);
    PagedModel<T> SearchWithPagination(string? searchQuery, int page, int pageSize);
}
