using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;
using WebApplication1.Helpers.SearchEngine.Abstractions;
using WebApplication1.Implementation.ViewModels;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

interface IConvertableToErrorVm
{
    ErrorVm GetErrorVm();
    int GetHttpStatusCode();
}

sealed class EntityNotFoundInTheDatabaseException : Exception, IConvertableToErrorVm
{
    private Guid _entityId;
    
    public EntityNotFoundInTheDatabaseException(Guid entityId)
        : base("Entity was not found in the database.")
    {
        _entityId = entityId;
        Data["EntityId"] = entityId;
    }

    public ErrorVm GetErrorVm()
    {
        return new ErrorVmBuilder()
            .WithError("Id", "Entity was not found in the database.",_entityId.ToString())
            .Build();
    }

    public int GetHttpStatusCode()
    {
        return 404;
    }
}

sealed class OneOrMoreEntitiesNotFoundInTheDatabaseException : Exception, IConvertableToErrorVm
{
    private readonly Guid[] _entitiesIds;
    
    public OneOrMoreEntitiesNotFoundInTheDatabaseException(params Guid[] entitiesIds)
        : base("Entity was not found in the database.")
    {
        _entitiesIds = entitiesIds;
        Data["EntitiesIds"] = _entitiesIds;
    }
    
    public ErrorVm GetErrorVm()
    {
        var errorVm = new ErrorVm();

        foreach (var entityId in _entitiesIds)
        {
            errorVm.AddError("Id", "Entity was not found in the database.", entityId.ToString());
        }

        return errorVm;
    }
    
    public int GetHttpStatusCode()
    {
        return 404;
    }
}

public class ProductsRepository : IRepository<Product>
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
        product.DefaultPrice = newEntityState.DefaultPrice;
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
            .AsNoTracking()
            .ToArray();

        var notFoundEntitiesIds = ids.Except(products.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return new PagedModel<Product>(products, page, pageSize, products.Count());
    }

    public IEnumerable<Product> Search(string? searchQuery)
    {
        return _searchEngine
            .ExecuteEngine(GetProductsSource(), searchQuery ?? "")
            .ToArray();
    }

    public PagedModel<Product> SearchWithPagination(string? searchQuery, int page, int pageSize)
    {
        var products = _searchEngine
            .ExecuteEngine(GetProductsSource(), searchQuery ?? string.Empty)
            .ToArray();

        return new PagedModel<Product>(products, page, pageSize, products.Length);
    }

    private IQueryable<Product> GetProductsSource()
    {
        return _dbContext.Products
            .Include(x => x.ProductCharacteristics);
    }
}
