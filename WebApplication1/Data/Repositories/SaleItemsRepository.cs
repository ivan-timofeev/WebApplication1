using WebApplication1.Abstractions.Data;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Common.Exceptions;
using WebApplication1.Common.Extensions;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class SaleItemsRepository : ISaleItemsRepository
{
    private readonly WebApplicationDbContext _dbContext;

    public SaleItemsRepository(WebApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public SaleItem Read(Guid id)
    {
        var saleItem = _dbContext.SaleItems
            .FirstOrDefault(x => x.Id == id);

        if (saleItem is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(SaleItem), id);
        }

        return saleItem;
    }

    public void Update(Guid entityId, SaleItem newEntityState)
    {
        var saleItem = _dbContext.SaleItems
            .FirstOrDefault(x => x.Id == entityId)
            .ThrowIfNotFound(entityId);

        saleItem.Quantity = newEntityState.Quantity;
        saleItem.SellingPrice = newEntityState.SellingPrice;
        saleItem.PurchasePrice = newEntityState.PurchasePrice;

        _dbContext.SaveChanges();
    }

    public IEnumerable<SaleItem> Read(IEnumerable<Guid> ids)
    {
        var saleItems = _dbContext.SaleItems
            .Where(x => ids.Contains(x.Id))
            .ToArray();

        var notFoundEntitiesIds = ids
            .Except(saleItems.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(SaleItem), notFoundEntitiesIds);
        }

        return saleItems.ToArray();
    }
}
