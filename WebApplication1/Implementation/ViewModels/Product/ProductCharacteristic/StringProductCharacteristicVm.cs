using System.Text.Json.Serialization;

namespace WebApplication1.ViewModels;

public class StringProductCharacteristicVm : ProductCharacteristicVm
{
    public string Value { get; set; }
}
