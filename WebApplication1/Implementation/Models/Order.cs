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

    public OrderStateEnum ActualOrderState => OrderStateHierarchical
        .OrderByDescending(x => x.SerialNumber)
        .First()
        .State;

    public ICollection<OrderStateHierarchicalItem> OrderStateHierarchical { get; set; }
        = new List<OrderStateHierarchicalItem>();
}

[Owned]
public class OrderStateHierarchicalItem
{
    public Guid Id { get; set; }
    public int SerialNumber { get; set; }
    public DateTime EnteredDateTimeUtc { get; set; }
    public OrderStateEnum State { get; set; }
    public string? Description { get; set; }
}

public enum OrderStateEnum
{
    Creating,
    ConfirmedByUser,
    ProcessingOrder,
    AssemblingOrder,
    AwaitingPayment,
    AwaitingForDelivery,
    DeliveryInProgress,
    Completed,
    Canceled = -1
}
