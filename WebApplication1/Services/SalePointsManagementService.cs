using AutoMapper;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Models;
using WebApplication1.Abstractions.Services;
using WebApplication1.Common.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services;

public class SalePointsManagementService : ISalePointsManagementService
{
    private readonly ISalePointsRepository _salePointsRepository;
    private readonly IMapper _mapper;

    public SalePointsManagementService(
        ISalePointsRepository salePointsRepository,
        IMapper mapper)
    {
        _salePointsRepository = salePointsRepository;
        _mapper = mapper;
    }
    
    public Guid CreateSalePoint(SalePointCreateVm model)
    {
        var salePoint = _mapper.Map<SalePoint>(model);
        var createdSalePointId = _salePointsRepository.Create(salePoint);

        return createdSalePointId;
    }

    public SalePointVm GetSalePoint(Guid salePointId)
    {
        var salePoint = _salePointsRepository.Read(salePointId);
        var salePointVm = _mapper.Map<SalePointVm>(salePoint);

        return salePointVm;
    }

    public void UpdateSalePoint(Guid salePointId, SalePointUpdateVm model)
    {
        var newSalePointState = _mapper.Map<SalePoint>(model);
        _salePointsRepository.Update(salePointId, newSalePointState);
    }

    public void DeleteSalePoint(Guid salePointId)
    {
        _salePointsRepository.Delete(salePointId);
    }

    public PagedModel<SalePointVm> SearchSalePoints(SearchEngineFilter? filter, int page, int pageSize)
    {
        var salePoints = _salePointsRepository
            .SearchWithPagination(filter, page, pageSize)
            .MapTo<SalePointVm>();

        return salePoints;
    }
}
