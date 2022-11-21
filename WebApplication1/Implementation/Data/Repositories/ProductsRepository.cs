using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;
using WebApplication1.Helpers.SearchEngine.Abstractions;
using WebApplication1.Implementation.ViewModels;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

interface IMiddlewareException
{
    ErrorVm GetErrorVm();
}

sealed class EntityNotFoundInTheDatabaseException : Exception, IMiddlewareException
{
    public EntityNotFoundInTheDatabaseException(Guid entityId)
        : base("Entity was not found in the database.")
    {
        Data["EntityId"] = entityId;

        var t = new ModelStateDictionary();
    }

    public ErrorVm GetErrorVm()
    {
        return new ErrorVmBuilder()
            .WithError("EntityId", "")
            .Build();
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
        return GetProductsSource()
            .AsNoTracking()
            .First(x => x.Id == id);
    }

    public Product Update(Guid entityId, Product newEntityState)
    {
        var product = _dbContext.Products.First(x => x.Id == entityId);

        product.Name = newEntityState.Name;
        product.Description = newEntityState.Description;
        product.DefaultPrice = newEntityState.DefaultPrice;
        product.ProductCharacteristics = newEntityState.ProductCharacteristics;

        _dbContext.SaveChanges();

        return product;
    }

    public void Delete(Guid entityId)
    {
        var source = _dbContext.Products
            .Include(x => x.ProductCharacteristics);

        var product = source.First(x => x.Id == entityId);
        product.IsDeleted = true;
        product.DeletedDateTimeUtc = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public IEnumerable<Product> Read(IEnumerable<Guid> ids)
    {
        var products = GetProductsSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking();

        if (products.Count() != ids.Count())
        {
            var exception = new Exception("One or more products was not found in the database");
            exception.Data.Add("NotFoundEntities", ids.Except(products.Select(x => x.Id)));
            
            throw exception;
        }

        return products.ToArray();
    }

    public PagedModel<Product> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var products = GetProductsSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToArray();

        if (products.Length != ids.Count())
        {
            var exception = new Exception("One or more products was not found in the database");
            exception.Data.Add("NotFoundEntities", ids.Except(products.Select(x => x.Id)));
            
            throw exception;
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
