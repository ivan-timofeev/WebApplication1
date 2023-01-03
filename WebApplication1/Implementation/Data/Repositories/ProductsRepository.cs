using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Common.SearchEngine.Abstractions;
using WebApplication1.Models;

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

    public Product Create(Product entity)
    {
        _dbContext.Products.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public Product Read(Guid id)
    {
        var product = GetProductsSource()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (product is null)
        {
            throw new EntityNotFoundInTheDatabaseException(id);
        }

        return product;
    }

    public Product Update(Guid entityId, Product newEntityState)
    {
        var product = GetProductsSource()
            .FirstOrDefault(x => x.Id == entityId);
        
        if (product is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }

        product.Name = newEntityState.Name;
        product.Description = newEntityState.Description;
        product.ProductCharacteristics = newEntityState.ProductCharacteristics;

        _dbContext.SaveChanges();

        return product;
    }

    public void Delete(Guid entityId)
    {
        var product = GetProductsSource().FirstOrDefault(x => x.Id == entityId);
        
        if (product is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
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

        var notFoundEntitiesIds = ids.Except(products.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return products.ToArray();
    }

    public PagedModel<Product> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var products = GetProductsSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking();

        var notFoundEntitiesIds = ids.Except(products.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return PagedModel<Product>.Paginate(products, page, pageSize);
    }

    public IEnumerable<Product> Search(string? searchQuery)
    {
        return _searchEngine
            .ExecuteEngine(GetProductsSource(), searchQuery ?? "")
            .AsNoTracking()
            .ToArray();
    }

    public PagedModel<Product> SearchWithPagination(string? searchQuery, int page, int pageSize)
    {
        var products = _searchEngine
            .ExecuteEngine(GetProductsSource(), searchQuery ?? string.Empty)
            .AsNoTracking();

        return PagedModel<Product>.Paginate(products, page, pageSize);
    }

    private IQueryable<Product> GetProductsSource()
    {
        return _dbContext.Products
            .Include(x => x.ProductCharacteristics);
    }
}
