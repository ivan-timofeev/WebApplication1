using WebApplication1.Abstraction.Models;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Data.Repositories;

public interface ISalePointsRepository : IRepository<SalePoint>
{
    /// <exception cref="Exception">An exception occurs if the point of sale does not contain any provided products.</exception>
    void EnsureSalePointContainsTheseProducts(Guid salePointId, params Guid[] productsIds);
}
