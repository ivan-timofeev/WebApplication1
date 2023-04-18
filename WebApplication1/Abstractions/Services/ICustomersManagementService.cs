using WebApplication1.Abstractions.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Abstractions.Services;

public interface ICustomersManagementService
{
    Guid CreateCustomer(CustomerCreateVm model);
    CustomerVm GetCustomer(Guid customerId);
    void UpdateCustomer(Guid customerId, CustomerUpdateVm model);
    void DeleteCustomer(Guid customerId);
    public PagedModel<CustomerVm> SearchCustomers(SearchEngineFilter? filter, int page, int pageSize);
    public PersonalAreaVm GetCustomerOrders(Guid customerId);
}
