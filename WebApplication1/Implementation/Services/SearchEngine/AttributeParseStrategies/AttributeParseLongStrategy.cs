
using WebApplication1.Abstraction.Services.SearchEngine;

namespace WebApplication1.Services.SearchEngine.AttributeParseStrategies;

public class AttributeParseLongStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(long);

    public object ParseAttribute(string attributeValue)
    {
        return long.Parse(attributeValue);
    }
}