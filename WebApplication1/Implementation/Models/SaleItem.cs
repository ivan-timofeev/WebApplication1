using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class SaleItem : DomainModel
{
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    
    public Guid SalePointId { get; set; }
    public virtual SalePoint SalePoint { get; set; }
    
    public int Quantity { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal? PurchasePrice { get; set; }
}
