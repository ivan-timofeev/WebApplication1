using WebApplication1.Abstraction.Models;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Data.Repositories;

public interface ICustomerOrdersRepository
{
    IEnumerable<Order> GetCustomerOrders(Customer customer);
}
