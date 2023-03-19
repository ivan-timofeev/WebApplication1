using WebApplication1.Models;

namespace WebApplication1.Abstraction.Services;

public interface IUpdateOrderStateStrategyResolver
{
    IUpdateOrderStateStrategy ResolveStrategy(OrderStateEnum actualOrderState, OrderStateEnum newOrderState);
}