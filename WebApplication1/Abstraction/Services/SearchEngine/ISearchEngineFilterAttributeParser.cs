
namespace WebApplication1.Abstraction.Services.SearchEngine;

public interface ISearchEngineFilterAttributeParser
{
    object ParseAttribute(string attributeValue, Type attributeType);
}
