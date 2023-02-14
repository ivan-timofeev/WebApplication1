namespace WebApplication1.Common.SearchEngine;

public class AttributeParseFloatStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(float);

    public object ParseAttribute(string attributeValue)
    {
        return float.Parse(attributeValue);
    }
}