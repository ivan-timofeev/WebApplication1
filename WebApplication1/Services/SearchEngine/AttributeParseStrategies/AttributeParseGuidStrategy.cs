using WebApplication1.Abstractions.Services.SearchEngine;

namespace WebApplication1.Services.SearchEngine.AttributeParseStrategies;

public class AttributeParseGuidStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(Guid);

    public object ParseAttribute(string attributeValue)
    {
        return Guid.Parse(attributeValue);
    }
}
