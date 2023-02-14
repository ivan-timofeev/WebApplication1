namespace WebApplication1.Common.SearchEngine;

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
