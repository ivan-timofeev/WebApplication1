using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class CustomerOrdersRepository : ICustomerOrdersRepository
{
    private readonly WebApplicationDbContext _dbContext;

    public CustomerOrdersRepository(
        WebApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Order> GetCustomerOrders(Customer customer)
    {
        return _dbContext.Orders
            .Include(x => x.OrderedItems)
            .ThenInclude(x => x.SaleItem)
            .ThenInclude(x => x.Product)
            .Where(x => x.CustomerId == customer.Id)
            .ToArray();
    }
}
