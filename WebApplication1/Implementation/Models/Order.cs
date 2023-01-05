using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class Order : DomainModel
{
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }

    public Guid SalePointId { get; set; }
    public virtual SalePoint SalePoint { get; set; }

    public ICollection<OrderItem> OrderedItems { get; set; }
        = new List<OrderItem>();

    public OrderStateEnum ActualOrderState => GetActualOrderState();

    public ICollection<OrderStateHierarchicalItem> OrderStateHierarchical { get; set; }
        = new List<OrderStateHierarchicalItem>();

    public OrderStateEnum GetActualOrderState()
        => OrderStateHierarchical
            .OrderByDescending(x => x.SerialNumber)
            .First()
            .State;
}

[Owned]
public class OrderStateHierarchicalItem
{
    public Guid Id { get; set; }
    public int SerialNumber { get; set; }
    public DateTime EnteredDateTimeUtc { get; set; }
    public OrderStateEnum State { get; set; }
    public string? EnterDescription { get; set; }
    public string? Details { get; set; }
}

public enum OrderStateEnum
{
    Creating,
    AwaitingAssembling,
    Assembling,
    AwaitingPayment,
    AwaitingForDelivery,
    DeliveryInProgress,
    Completed,
    Canceled = -1
}
