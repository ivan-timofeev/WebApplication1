namespace WebApplication1.Common.SearchEngine;

public class AttributeParseIntStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(int);

    public object ParseAttribute(string attributeValue)
    {
        return int.Parse(attributeValue);
    }
}
