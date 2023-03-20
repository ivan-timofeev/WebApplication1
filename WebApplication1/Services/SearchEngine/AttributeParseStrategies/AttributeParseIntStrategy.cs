using WebApplication1.Abstractions.Services.SearchEngine;

namespace WebApplication1.Services.SearchEngine.AttributeParseStrategies;

public class AttributeParseIntStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(int);

    public object ParseAttribute(string attributeValue)
    {
        return int.Parse(attributeValue);
    }
}
