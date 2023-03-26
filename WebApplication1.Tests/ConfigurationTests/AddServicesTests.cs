using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Common.Extensions;
using WebApplication1.Services.OrdersManagementService;
using WebApplication1.Services.OrdersManagementService.UpdateOrderStateStrategies;

namespace WebApplication1.Tests.ConfigurationTests;

public class AddServicesTests
{
    [Fact]
    public void Test_AddOrdersManagementService_ServicesShouldContainsAllRequiredParts()
    {
        var services = new ServiceCollection();


        services.AddOrdersManagementService();


        services
            .AssertServiceRegistered(typeof(OrdersManagementService))
            .AssertServiceRegistered(typeof(UpdateOrderStateStrategyResolver))
            // Order update strategies
            .AssertServiceRegistered(typeof(UpdateOrderStateStrategyToAssembling))
            .AssertServiceRegistered(typeof(UpdateOrderStateStrategyToAwaitingCustomerPickup))
            .AssertServiceRegistered(typeof(UpdateOrderStateStrategyToAwaitingForDelivery))
            .AssertServiceRegistered(typeof(UpdateOrderStateStrategyToCancelled))
            .AssertServiceRegistered(typeof(UpdateOrderStateStrategyToCompleted))
            .AssertServiceRegistered(typeof(UpdateOrderStateStrategyToDeliveryInProgress));
    }
}
