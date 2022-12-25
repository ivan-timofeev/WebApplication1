using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;
using WebApplication1.Common.SearchEngine.Abstractions;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class SalePointsRepository : IRepository<SalePoint>
{
    private readonly IRepository<Product> _productsRepository;
    private readonly WebApplicationDbContext _dbContext;
    private readonly ISearchEngine _searchEngine;

    public SalePointsRepository(
        IRepository<Product> productsRepository,
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

        return entity;
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

        var notFoundEntitiesIds = ids.Except(salePoints.Select(x => x.Id));

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

        var notFoundEntitiesIds = ids.Except(salePoints.Select(x => x.Id));

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

    private IQueryable<SalePoint> GetSalePointsSource()
    {
        return _dbContext.SalePoints
            .Include(x => x.SaleItems)!
            .ThenInclude(x => x.Product);
    }

    /// <exception cref="OneOrMoreEntitiesNotFoundInTheDatabaseException"></exception>
    private void EnsureProductsAreExists(SalePoint salePoint)
    {
        if (salePoint.SaleItems == null || !salePoint.SaleItems.Any())
            return;

        var productIds = salePoint.SaleItems
            .Select(x => x.ProductId);

        var products = _productsRepository
            .Read(productIds)
            .ToArray();

        foreach (var saleItem in salePoint.SaleItems)
        {
            saleItem.Product = products.Single(x => x.Id == saleItem.ProductId);
        }
    }
}
