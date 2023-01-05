using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Common.SearchEngine.Abstractions;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class SalePointsRepository : ISalePointsRepository
{
    private readonly IProductsRepository _productsRepository;
    private readonly WebApplicationDbContext _dbContext;
    private readonly ISearchEngine _searchEngine;

    public SalePointsRepository(
        IProductsRepository productsRepository,
        WebApplicationDbContext dbContext,
        ISearchEngine searchEngine)
    {
        _productsRepository = productsRepository;
        _dbContext = dbContext;
        _searchEngine = searchEngine;
    }

    public SalePoint Create(SalePoint entity)
    {
        EnsureProductsAreExists(entity);
        
        _dbContext.SalePoints.Add(entity);
        _dbContext.SaveChanges();

        return Read(entity.Id);
    }

    public SalePoint Read(Guid id)
    {
        var salePoint = GetSalePointsSource()
            .FirstOrDefault(x => x.Id == id);

        if (salePoint is null)
        {
            throw new EntityNotFoundInTheDatabaseException(id);
        }

        return salePoint;
    }

    public SalePoint Update(Guid entityId, SalePoint newEntityState)
    {
        var salePoint = GetSalePointsSource()
            .FirstOrDefault(x => x.Id == entityId);
        
        if (salePoint is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }
        
        EnsureProductsAreExists(newEntityState);

        salePoint.Name = newEntityState.Name;
        salePoint.Address = newEntityState.Address;
        salePoint.SaleItems = newEntityState.SaleItems;

        _dbContext.SaveChanges();

        return salePoint;
    }

    public void Delete(Guid entityId)
    {
        var salePoint = GetSalePointsSource().FirstOrDefault(x => x.Id == entityId);
        
        if (salePoint is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }
        
        salePoint.IsDeleted = true;
        salePoint.DeletedDateTimeUtc = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public IEnumerable<SalePoint> Read(IEnumerable<Guid> ids)
    {
        var salePoints = GetSalePointsSource()
            .Where(x => ids.Contains(x.Id))
            .ToArray();

        var notFoundEntitiesIds = ids
            .Except(salePoints.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return salePoints.ToArray();
    }

    public PagedModel<SalePoint> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var salePoints = GetSalePointsSource()
            .Where(x => ids.Contains(x.Id));

        var notFoundEntitiesIds = ids
            .Except(salePoints.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return PagedModel<SalePoint>.Paginate(salePoints, page, pageSize);
    }

    public IEnumerable<SalePoint> Search(string? searchQuery)
    {
        return _searchEngine
            .ExecuteEngine(GetSalePointsSource(), searchQuery ?? "")
            .ToArray();
    }

    public PagedModel<SalePoint> SearchWithPagination(string? searchQuery, int page, int pageSize)
    {
        var salePoints = _searchEngine
            .ExecuteEngine(GetSalePointsSource(),searchQuery ?? string.Empty);

        return PagedModel<SalePoint>.Paginate(salePoints, page, pageSize);
    }

    public void EnsureSalePointContainsTheseProducts(Guid salePointId, params Guid[] productsIds)
    {
        var salePoint = Read(salePointId);

        var foundProductIds = salePoint.SaleItems
            .Where(x => productsIds.Contains(x.ProductId))
            .Select(x => x.ProductId)
            .ToArray();
        var notFoundProductIds = productsIds
            .Except(foundProductIds)
            .ToArray();

        if (notFoundProductIds.Any())
            throw new Exception("ТОРГОВАЯ ТОЧКА НЕ СОДЕРЖИТ УКАЗАННЫХ ПРОДУКТОВ (TODO: сделать отдельный експешен)");
    }

    private IQueryable<SalePoint> GetSalePointsSource()
    {
        return _dbContext.SalePoints
            .Include(x => x.SaleItems)!
            .ThenInclude(x => x.Product);
    }

    /// <exception cref="OneOrMoreEntitiesNotFoundInTheDatabaseException"></exception>
    private void EnsureProductsAreExists(SalePoint salePoint)
    {
        if (!salePoint.SaleItems.Any())
            return;

        var productIds = salePoint.SaleItems
            .Select(x => x.ProductId);

        // TODO: выглядит чудовищно неэффективным, хочется метод типа EnsureExists или AreExists
        var products = _productsRepository
            .Read(productIds)
            .ToArray();
    }
}
