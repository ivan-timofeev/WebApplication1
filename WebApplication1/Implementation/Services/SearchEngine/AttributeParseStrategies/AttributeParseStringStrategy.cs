
using WebApplication1.Abstraction.Services.SearchEngine;

namespace WebApplication1.Services.SearchEngine.AttributeParseStrategies;

public class AttributeParseStringStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(string);

    public object ParseAttribute(string attributeValue)
    {
        return !string.IsNullOrWhiteSpace(attributeValue)
            ? attributeValue
            : string.Empty;
    }
}
