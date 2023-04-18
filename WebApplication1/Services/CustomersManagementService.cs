using AutoMapper;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Models;
using WebApplication1.Abstractions.Services;
using WebApplication1.Common.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services;

public class CustomersManagementService : ICustomersManagementService
{
    private readonly IMapper _mapper;
    private readonly ICustomersRepository _customersRepository;

    public CustomersManagementService(
        IMapper mapper,
        ICustomersRepository customersRepository)
    {
        _mapper = mapper;
        _customersRepository = customersRepository;
    }
    
    public Guid CreateCustomer(CustomerCreateVm model)
    {
        var customer = _mapper.Map<Customer>(model);
        var createdCustomerId = _customersRepository.Create(customer);

        return createdCustomerId;
    }

    public CustomerVm GetCustomer(Guid customerId)
    {
        var customer = _customersRepository.Read(customerId);
        var customerVm = _mapper.Map<CustomerVm>(customer);

        return customerVm;
    }

    public void UpdateCustomer(Guid customerId, CustomerUpdateVm model)
    {
        var newCustomerState = _mapper.Map<Customer>(model);
        _customersRepository.Update(customerId, newCustomerState);
    }

    public void DeleteCustomer(Guid customerId)
    {
        _customersRepository.Delete(customerId);
    }

    public PagedModel<CustomerVm> SearchCustomers(SearchEngineFilter? filter, int page, int pageSize)
    {
        var customers = _customersRepository
            .SearchWithPagination(filter, page, pageSize)
            .MapTo<CustomerVm>();

        return customers;
    }
}
