using WebApplication1.Abstractions.Services.SearchEngine;

namespace WebApplication1.Services.SearchEngine.AttributeParseStrategies;

public class AttributeParseLongStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(long);

    public object ParseAttribute(string attributeValue)
    {
        return long.Parse(attributeValue);
    }
}