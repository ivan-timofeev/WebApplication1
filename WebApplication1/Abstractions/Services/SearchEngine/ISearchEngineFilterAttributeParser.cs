
namespace WebApplication1.Abstractions.Services.SearchEngine;

public interface ISearchEngineFilterAttributeParser
{
    object ParseAttribute(string attributeValue, Type attributeType);
}
