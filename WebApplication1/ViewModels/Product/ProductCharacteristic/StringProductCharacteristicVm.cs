#nullable disable
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public class StringProductCharacteristicVm : ProductCharacteristicVm
{
    [Required]
    public string Value { get; set; }
}
