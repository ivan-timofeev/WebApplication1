namespace WebApplication1.Common.SearchEngine;

public interface IAttributeParseStrategy
{
    Type AssignedType { get; }
    object ParseAttribute(string attributeValue);
}
