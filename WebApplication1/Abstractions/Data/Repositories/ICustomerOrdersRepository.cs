using WebApplication1.Models;

namespace WebApplication1.Abstractions.Data.Repositories;

public interface ICustomerOrdersRepository
{
    IEnumerable<Order> GetCustomerOrders(Customer customer);
}
