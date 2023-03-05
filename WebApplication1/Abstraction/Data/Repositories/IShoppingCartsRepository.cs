using WebApplication1.Abstraction.Models;
using WebApplication1.Common.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Data.Repositories;

public interface IShoppingCartsRepository : ICrudRepository<ShoppingCart>
{
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    void UpdateShoppingCartItem(Guid customerId, Guid saleItemId, int quantity);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    ShoppingCart ReadByCustomer(Guid customerId);
}
