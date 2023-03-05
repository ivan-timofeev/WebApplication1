using WebApplication1.Common.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Data.Repositories;

public interface ISaleItemsRepository
{
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    SaleItem Read(Guid id);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    void Update(Guid entityId, SaleItem newEntityState);
    /// <exception cref="OneOrMoreEntitiesNotFoundInTheDatabaseException"></exception>
    IEnumerable<SaleItem> Read(IEnumerable<Guid> ids);
}
