using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Common.Exceptions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Data.Repositories;

public class CustomersRepository : ICustomersRepository
{
    private readonly WebApplicationDbContext _dbContext;
    private readonly ISearchEngine _searchEngine;

    public CustomersRepository(
        WebApplicationDbContext dbContext,
        ISearchEngine searchEngine)
    {
        _dbContext = dbContext;
        _searchEngine = searchEngine;
    }

    public Customer Create(Customer entity)
    {
        _dbContext.Customers.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public Customer Read(Guid id)
    {
        var customer = GetCustomersSource()
            .FirstOrDefault(x => x.Id == id);

        if (customer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(id);
        }

        return customer;
    }

    public void Update(Guid entityId, Customer newEntityState)
    {
        var customer = GetCustomersSource()
            .FirstOrDefault(x => x.Id == entityId);
        
        if (customer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }

        customer.Orders = newEntityState.Orders;

        _dbContext.SaveChanges();
    }

    public void Delete(Guid entityId)
    {
        var customer = GetCustomersSource().FirstOrDefault(x => x.Id == entityId);
        
        if (customer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }
        
        customer.IsDeleted = true;
        customer.DeletedDateTimeUtc = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public IEnumerable<Customer> Read(IEnumerable<Guid> ids)
    {
        var customers = GetCustomersSource()
            .Where(x => ids.Contains(x.Id))
            .ToArray();

        var notFoundEntitiesIds = ids
            .Except(customers.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return customers.ToArray();
    }

    public PagedModel<Customer> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var customers = GetCustomersSource()
            .Where(x => ids.Contains(x.Id));

        var notFoundEntitiesIds = ids
            .Except(customers.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return PagedModel<Customer>.Paginate(customers, page, pageSize);
    }

    public IEnumerable<Customer> Search(SearchEngineFilter? filter)
    {
        return _searchEngine
            .ExecuteEngine(GetCustomersSource(), filter)
            .AsNoTracking()
            .ToArray();
    }

    public PagedModel<Customer> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize)
    {
        var customers = _searchEngine
            .ExecuteEngine(GetCustomersSource(), filter)
            .AsNoTracking();

        return PagedModel<Customer>.Paginate(customers, page, pageSize);
    }

    private IQueryable<Customer> GetCustomersSource()
    {
        return _dbContext.Customers;
    }
}
