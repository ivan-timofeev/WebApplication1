using WebApplication1.Abstractions.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Abstractions.Services;

public interface IProductsManagementService
{
    public Guid CreateProduct(ProductCreateVm model);
    public ProductVm GetProduct(Guid productId);
    public void UpdateProduct(Guid productId, ProductUpdateVm model);
    public void DeleteProduct(Guid productId);
    public PagedModel<ProductVm> SearchProducts(SearchEngineFilter? filter, int page, int pageSize);
}
