using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class Order : DomainModel
{
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    
    public ICollection<SaleItem> OrderedItems { get; set; }
    public OrderStateEnum ActualOrderState { get; set; }
    
    public ICollection<OrderStateHierarchicalItem> OrderStateHierarchical { get; set; }
}

[Owned]
public class OrderStateHierarchicalItem
{
    public Guid Id { get; set; }
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
