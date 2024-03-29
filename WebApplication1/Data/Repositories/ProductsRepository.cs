using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions.Data;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Models;
using WebApplication1.Abstractions.Services.SearchEngine;
using WebApplication1.Common.Exceptions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Data.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly WebApplicationDbContext _dbContext;
    private readonly ISearchEngine _searchEngine;

    public ProductsRepository(
        WebApplicationDbContext dbContext,
        ISearchEngine searchEngine)
    {
        _dbContext = dbContext;
        _searchEngine = searchEngine;
    }

    public Guid Create(Product entity)
    {
        _dbContext.Products.Add(entity);
        _dbContext.SaveChanges();

        return entity.Id;
    }

    public Product Read(Guid id)
    {
        var product = GetProductsSource()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (product is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Product), id);
        }

        return product;
    }

    public void Update(Guid entityId, Product newEntityState)
    {
        var product = GetProductsSource()
            .FirstOrDefault(x => x.Id == entityId);
        
        if (product is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Product), entityId);
        }

        product.Name = newEntityState.Name;
        product.Description = newEntityState.Description;
        product.ProductCharacteristics = newEntityState.ProductCharacteristics;

        _dbContext.SaveChanges();
    }

    public void Delete(Guid entityId)
    {
        var product = GetProductsSource().FirstOrDefault(x => x.Id == entityId);
        
        if (product is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Product), entityId);
        }
        
        product.IsDeleted = true;
        product.DeletedDateTimeUtc = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public IEnumerable<Product> Read(IEnumerable<Guid> ids)
    {
        var products = GetProductsSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToArray();

        var notFoundEntitiesIds = ids
            .Except(products.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(Product), notFoundEntitiesIds.ToArray());
        }

        return products.ToArray();
    }

    public PagedModel<Product> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var products = GetProductsSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking();

        var notFoundEntitiesIds = ids
            .Except(products.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(Product), notFoundEntitiesIds);
        }

        return PagedModel<Product>.Paginate(products, page, pageSize);
    }

    public IEnumerable<Product> Search(SearchEngineFilter? filter)
    {
        return _searchEngine
            .ExecuteEngine(GetProductsSource(), filter)
            .AsNoTracking()
            .ToArray();
    }

    public PagedModel<Product> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize)
    {
        var products = _searchEngine
            .ExecuteEngine(GetProductsSource(), filter)
            .AsNoTracking();

        return PagedModel<Product>.Paginate(products, page, pageSize);
    }

    private IQueryable<Product> GetProductsSource()
    {
        return _dbContext.Products
            .Include(x => x.ProductCharacteristics);
    }
}
