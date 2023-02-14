using System.Globalization;
using WebApplication1.Abstraction.Services.SearchEngine;

namespace WebApplication1.Services.SearchEngine.AttributeParseStrategies;

public class AttributeParseDateTimeStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(DateTime);

    public object ParseAttribute(string attributeValue)
    {
        return DateTime.Parse(attributeValue, CultureInfo.InvariantCulture);
    }
}
