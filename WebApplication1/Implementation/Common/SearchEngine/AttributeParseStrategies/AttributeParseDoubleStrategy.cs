namespace WebApplication1.Common.SearchEngine;

public class AttributeParseDoubleStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(float);

    public object ParseAttribute(string attributeValue)
    {
        return float.Parse(attributeValue);
    }
}
