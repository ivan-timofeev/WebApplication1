namespace WebApplication1.Abstraction.Models;

public interface ICrudRepository<T>
    where T : class, IDomainModel
{
    T Create(T entity);
    T Read(Guid id);
    T Update(Guid entityId, T newEntityState);
    void Delete(Guid entityId);

    T? TryCreate(T entity)
    {
        try
        {
            return Create(entity);
        }
        catch
        {
            return null;
        }
    }

    T? TryRead(Guid id)
    {
        try
        {
            return Read(id);
        }
        catch
        {
            return null;
        }
    }

    T? TryUpdate(Guid entityId, T newEntityState)
    {
        try
        {
            return Update(entityId, newEntityState);
        }
        catch
        {
            return null;
        }
    }

    bool TryDelete(Guid entityId)
    {
        try
        {
            Delete(entityId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    IEnumerable<T> Read(IEnumerable<Guid> ids);
    PagedModel<T> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize);
}
