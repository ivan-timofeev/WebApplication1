using WebApplication1.Data.Repositories;

namespace WebApplication1.Abstraction.Models;

public interface ICrudRepository<T>
    where T : class, IDomainModel
{
    T Create(T entity);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    T Read(Guid id);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    T Update(Guid entityId, T newEntityState);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
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

    
    /// <exception cref="OneOrMoreEntitiesNotFoundInTheDatabaseException"></exception>
    IEnumerable<T> Read(IEnumerable<Guid> ids);
    /// <exception cref="OneOrMoreEntitiesNotFoundInTheDatabaseException"></exception>
    PagedModel<T> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize);
}
