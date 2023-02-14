namespace WebApplication1.Common.SearchEngine;

public class AttributeParseLongStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(long);

    public object ParseAttribute(string attributeValue)
    {
        return long.Parse(attributeValue);
    }
}