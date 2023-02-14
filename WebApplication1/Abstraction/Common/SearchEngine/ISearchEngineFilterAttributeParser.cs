
namespace WebApplication1.Abstraction.Common.SearchEngine;

public interface ISearchEngineFilterAttributeParser
{
    object ParseAttribute(string attributeValue, Type attributeType);
}
