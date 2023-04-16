using AutoMapper;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Models;
using WebApplication1.Abstractions.Services;
using WebApplication1.Common.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services;

public class ProductsManagementService : IProductsManagementService
{
    private readonly IProductsRepository _productsRepository;
    private readonly IMapper _mapper;

    public ProductsManagementService(
        IProductsRepository productsRepository,
        IMapper mapper)
    {
        _productsRepository = productsRepository;
        _mapper = mapper;
    }
    
    public Guid CreateProduct(ProductCreateVm model)
    {
        var product = _mapper.Map<Product>(model);
        var createdProductId = _productsRepository.Create(product);

        return createdProductId;
    }

    public ProductVm GetProduct(Guid productId)
    {
        var product = _productsRepository.Read(productId);
        var productVm = _mapper.Map<ProductVm>(product);

        return productVm;
    }

    public void UpdateProduct(Guid productId, ProductUpdateVm model)
    {
        var newProductState = _mapper.Map<Product>(model);
        _productsRepository.Update(productId, newProductState);
    }

    public void DeleteProduct(Guid productId)
    {
        _productsRepository.Delete(productId);
    }

    public PagedModel<ProductVm> SearchProducts(SearchEngineFilter? filter, int page, int pageSize)
    {
        var products = _productsRepository
            .SearchWithPagination(filter, page, pageSize)
            .MapTo<ProductVm>();

        return products;
    }
}
