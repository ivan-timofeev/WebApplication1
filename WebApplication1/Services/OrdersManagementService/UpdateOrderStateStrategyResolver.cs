using WebApplication1.Abstractions.Services;
using WebApplication1.Common.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Services.OrdersManagementService;

public class UpdateOrderStateStrategyResolver
    : IUpdateOrderStateStrategyResolver
{
    private readonly IEnumerable<IUpdateOrderStateStrategy> _registeredStrategies;

    public UpdateOrderStateStrategyResolver(
        IEnumerable<IUpdateOrderStateStrategy> registeredStrategies)
    {
        _registeredStrategies = registeredStrategies;
    }

    public IUpdateOrderStateStrategy ResolveStrategy(OrderStateEnum actualOrderState, OrderStateEnum newOrderState)
    {
        var strategy = _registeredStrategies
            .Where(x => x.ToState == newOrderState 
                && x.FromStates.Contains(actualOrderState))
            .MaxBy(x => x.Priority);

        if (strategy is not null)
            return strategy;

        var errorVm = new ErrorVmBuilder()
            .WithGlobalError("Unable to transfer order from State-A to State-B.")
            .WithInfo("State-A", actualOrderState)
            .WithInfo("State-B", newOrderState)
            .Build();

        throw new BusinessErrorException("Unable to transfer order from State-A to State-B.",
            StatusCodes.Status400BadRequest, errorVm);
    }
}
