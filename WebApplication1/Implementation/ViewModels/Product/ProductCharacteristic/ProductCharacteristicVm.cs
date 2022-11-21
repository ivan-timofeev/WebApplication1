using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.ViewModels;

[JsonDerivedType(typeof(NumberProductCharacteristicVm), "number")]
[JsonDerivedType(typeof(StringProductCharacteristicVm), "text")]
public class ProductCharacteristicVm
{
    [Required] public string Name { get; set; } = null!;
}
