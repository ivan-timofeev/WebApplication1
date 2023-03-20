
namespace WebApplication1.Abstractions.Services.SearchEngine;

public interface IAttributeParseStrategy
{
    Type AssignedType { get; }
    object ParseAttribute(string attributeValue);
}
