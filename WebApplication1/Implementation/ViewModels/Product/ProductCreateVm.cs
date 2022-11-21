using System.ComponentModel.DataAnnotations;
using WebApplication1.Attributes;

namespace WebApplication1.ViewModels;

public class ProductCreateVm
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal? DefaultPrice { get; set; }
    
    [CanNotBe(typeof(ProductCharacteristicVm), ErrorMessage = "Product characteristic must have a type.")]
    public ICollection<ProductCharacteristicVm>? ProductCharacteristics { get; set; }
}
