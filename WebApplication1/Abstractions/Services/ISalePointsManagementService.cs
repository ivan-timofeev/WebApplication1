using WebApplication1.Abstractions.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Abstractions.Services;

public interface ISalePointsManagementService
{
    public Guid CreateSalePoint(SalePointCreateVm model);
    public SalePointVm GetSalePoint(Guid salePointId);
    public void UpdateSalePoint(Guid salePointId, SalePointUpdateVm model);
    public void DeleteSalePoint(Guid salePointId);
    public PagedModel<SalePointVm> SearchSalePoints(SearchEngineFilter? filter, int page, int pageSize);
}
