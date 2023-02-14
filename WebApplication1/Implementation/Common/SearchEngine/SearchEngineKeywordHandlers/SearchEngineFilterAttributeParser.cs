using AutoMapper.Internal;
using WebApplication1.Abstraction.Common.SearchEngine;

namespace WebApplication1.Common.SearchEngine;

public class SearchEngineFilterAttributeParser : ISearchEngineFilterAttributeParser
{
    private readonly IEnumerable<IAttributeParseStrategy> _attributeParseStrategies;

    public SearchEngineFilterAttributeParser(IEnumerable<IAttributeParseStrategy> attributeParseStrategies)
    {
        _attributeParseStrategies = attributeParseStrategies;
    }

    public object ParseAttribute(string attributeValue, Type attributeType)
    {
        var correctedAttributeType = attributeType.IsNullableType()
            ? attributeType.GetGenericArguments()[0]
            : attributeType;
        
        var parserStrategy = _attributeParseStrategies.Single(x => x.AssignedType == correctedAttributeType);
        return parserStrategy.ParseAttribute(attributeValue);
    }
}
