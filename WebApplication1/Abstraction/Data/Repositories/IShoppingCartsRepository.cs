using WebApplication1.Abstraction.Models;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Data.Repositories;

public interface IShoppingCartsRepository : ICrudRepository<ShoppingCart>
{
    void UpdateShoppingCartItem(Guid customerId, Guid saleItemId, int quantity);
}
