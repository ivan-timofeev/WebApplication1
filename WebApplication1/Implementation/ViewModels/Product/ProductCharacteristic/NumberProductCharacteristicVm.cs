using System.Text.Json.Serialization;

namespace WebApplication1.ViewModels;

public class NumberProductCharacteristicVm : ProductCharacteristicVm
{
    public decimal Value { get; set; }
}
