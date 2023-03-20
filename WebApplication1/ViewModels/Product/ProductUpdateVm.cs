using System.ComponentModel.DataAnnotations;
using WebApplication1.Abstractions.Common.Attributes;

namespace WebApplication1.ViewModels;

public record ProductUpdateVm(
    [Required]
    string Name,
    
    string? Description,
    
    [CanNotBe(typeof(ProductCharacteristicVm), ErrorMessage = "Product characteristic must have a type.")]
    IEnumerable<ProductCharacteristicVm>? ProductCharacteristics
);