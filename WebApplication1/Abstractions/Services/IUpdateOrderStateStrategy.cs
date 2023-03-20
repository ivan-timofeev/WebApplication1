using WebApplication1.Models;

namespace WebApplication1.Abstractions.Services;

public interface IUpdateOrderStateStrategy
{
    int Priority { get; }
    OrderStateEnum[] FromStates { get; }
    OrderStateEnum ToState { get; }

    void UpdateOrder(Order order);
}
