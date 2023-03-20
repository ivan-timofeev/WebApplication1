namespace WebApplication1.Models;

public class StringProductCharacteristic : ProductCharacteristic
{
    public string Value { get; set; }

    public StringProductCharacteristic()
    {
        Value = string.Empty;
    }
}
