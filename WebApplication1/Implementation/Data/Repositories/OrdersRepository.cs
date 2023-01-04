using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Common.SearchEngine.Abstractions;
using WebApplication1.Models;

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

        order.CustomerId = newEntityState.CustomerId;
        order.Customer = _customersRepository.Read(newEntityState.CustomerId);
        order.SalePointId = newEntityState.SalePointId;
        order.SalePoint = _salePointsRepository.Read(newEntityState.SalePointId);

        var orderProductIds = newEntityState.OrderedItems.Select(x => x.ProductId).ToArray();

        _salePointsRepository.EnsureSalePointContainsTheseProducts(
            newEntityState.SalePointId, 
            orderProductIds);

        order.OrderedItems = newEntityState.OrderedItems;
        order.OrderStateHierarchical = newEntityState.OrderStateHierarchical;

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

        var notFoundEntitiesIds = ids
            .Except(orders.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds);
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
        return _dbContext.Orders
            .Include(x => x.Customer)
            .Include(x => x.SalePoint)
            .Include(x => x.OrderStateHierarchical)
            .Include(x => x.OrderedItems)
            .ThenInclude(x => x.Product);
    }
}
