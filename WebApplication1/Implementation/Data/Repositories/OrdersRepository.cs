using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Common.Exceptions;
using WebApplication1.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Data.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly WebApplicationDbContext _dbContext;
    private readonly ISearchEngine _searchEngine;
    private readonly ICustomersRepository _customersRepository;
    private readonly ISalePointsRepository _salePointsRepository;

    public OrdersRepository(
        WebApplicationDbContext dbContext,
        ISearchEngine searchEngine,
        ICustomersRepository customersRepository,
        ISalePointsRepository salePointsRepository)
    {
        _dbContext = dbContext;
        _searchEngine = searchEngine;
        _customersRepository = customersRepository;
        _salePointsRepository = salePointsRepository;
    }

    public Guid Create(Order entity)
    {
        _dbContext.Orders.Add(entity);
        _dbContext.SaveChanges();

        return entity.Id;
    }

    public Order Read(Guid id)
    {
        var order = GetOrdersSource()
            .FirstOrDefault(x => x.Id == id);

        if (order is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Order), id);
        }

        return order;
    }

    public void Update(Guid orderId, Order newEntityState)
    {
        var order = GetOrdersSource()
            .FirstOrDefault(x => x.Id == orderId)
            .ThrowIfNotFound(orderId);

        order.OrderStateHierarchical = newEntityState.OrderStateHierarchical;

        _dbContext.SaveChanges();
    }

    public void Delete(Guid orderId)
    {
        var order = GetOrdersSource().FirstOrDefault(x => x.Id == orderId);
        
        if (order is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Order), orderId);
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

        var notFoundEntitiesIds = ids
            .Except(orders.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(Order), notFoundEntitiesIds);
        }

        return orders.ToArray();
    }

    public PagedModel<Order> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var orders = GetOrdersSource()
            .Where(x => ids.Contains(x.Id));

        var notFoundEntitiesIds = ids
            .Except(orders.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(Order), notFoundEntitiesIds.ToArray());
        }

        return PagedModel<Order>.Paginate(orders, page, pageSize);
    }

    public IEnumerable<Order> Search(SearchEngineFilter? filter)
    {
        return _searchEngine
            .ExecuteEngine(GetOrdersSource(), filter)
            .ToArray();
    }

    public PagedModel<Order> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize)
    {
        var orders = _searchEngine
            .ExecuteEngine(GetOrdersSource(), filter);

        return PagedModel<Order>.Paginate(orders, page, pageSize);
    }

    private IQueryable<Order> GetOrdersSource()
    {
        return _dbContext.Orders
            .Include(x => x.Customer)
            .Include(x => x.OrderStateHierarchical)
            .Include(x => x.OrderedItems)
            .ThenInclude(x => x.SaleItem);
    }
}
