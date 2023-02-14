namespace WebApplication1.Common.SearchEngine;

public class AttributeParseGuidStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(Guid);

    public object ParseAttribute(string attributeValue)
    {
        return Guid.Parse(attributeValue);
    }
}
