using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;
using WebApplication1.Common.SearchEngine.Abstractions;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class OrdersRepository : IRepository<Order>
{
    private readonly WebApplicationDbContext _dbContext;
    private readonly ISearchEngine _searchEngine;

    public OrdersRepository(
        WebApplicationDbContext dbContext,
        ISearchEngine searchEngine)
    {
        _dbContext = dbContext;
        _searchEngine = searchEngine;
    }

    public Order Create(Order entity)
    {
        _dbContext.Orders.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public Order Read(Guid id)
    {
        var order = GetOrdersSource()
            .FirstOrDefault(x => x.Id == id);

        if (order is null)
        {
            throw new EntityNotFoundInTheDatabaseException(id);
        }

        return order;
    }

    public Order Update(Guid entityId, Order newEntityState)
    {
        var order = GetOrdersSource()
            .FirstOrDefault(x => x.Id == entityId);
        
        if (order is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }

        _dbContext.SaveChanges();

        return order;
    }

    public void Delete(Guid entityId)
    {
        var order = GetOrdersSource().FirstOrDefault(x => x.Id == entityId);
        
        if (order is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }
        
        order.IsDeleted = true;
        order.DeletedDateTimeUtc = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public IEnumerable<Order> Read(IEnumerable<Guid> ids)
    {
        var orders = GetOrdersSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToArray();

        var notFoundEntitiesIds = ids.Except(orders.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return orders.ToArray();
    }

    public PagedModel<Order> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var orders = GetOrdersSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking();

        var notFoundEntitiesIds = ids.Except(orders.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return PagedModel<Order>.Paginate(orders, page, pageSize);
    }

    public IEnumerable<Order> Search(string? searchQuery)
    {
        return _searchEngine
            .ExecuteEngine(GetOrdersSource(), searchQuery ?? "")
            .AsNoTracking()
            .ToArray();
    }

    public PagedModel<Order> SearchWithPagination(string? searchQuery, int page, int pageSize)
    {
        var orders = _searchEngine
            .ExecuteEngine(GetOrdersSource(), searchQuery ?? string.Empty)
            .AsNoTracking();

        return PagedModel<Order>.Paginate(orders, page, pageSize);
    }

    private IQueryable<Order> GetOrdersSource()
    {
        return _dbContext.Orders;
    }
}
