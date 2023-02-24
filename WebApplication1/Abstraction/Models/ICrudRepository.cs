using WebApplication1.Common.Exceptions;

namespace WebApplication1.Abstraction.Models;

public interface ICrudRepository<T>
    where T : class, IDomainModel
{
    Guid Create(T entity);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    T Read(Guid id);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    void Update(Guid entityId, T newEntityState);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    void Delete(Guid entityId);

    Guid? TryCreate(T entity)
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

    void TryUpdate(Guid entityId, T newEntityState)
    {
        try
        {
            Update(entityId, newEntityState);
        }
        catch
        {
            // Ignored
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
