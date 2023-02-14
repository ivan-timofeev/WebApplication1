
namespace WebApplication1.Abstraction.Services.SearchEngine;

public interface IAttributeParseStrategy
{
    Type AssignedType { get; }
    object ParseAttribute(string attributeValue);
}
