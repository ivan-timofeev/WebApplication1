
using WebApplication1.Abstraction.Services.SearchEngine;

namespace WebApplication1.Services.SearchEngine.AttributeParseStrategies;

public class AttributeParseDoubleStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(float);

    public object ParseAttribute(string attributeValue)
    {
        return float.Parse(attributeValue);
    }
}
