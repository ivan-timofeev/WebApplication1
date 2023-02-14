using System.Globalization;

namespace WebApplication1.Common.SearchEngine;

public class AttributeParseDateTimeStrategy : IAttributeParseStrategy
{
    public Type AssignedType => typeof(DateTime);

    public object ParseAttribute(string attributeValue)
    {
        return DateTime.Parse(attributeValue, CultureInfo.InvariantCulture);
    }
}
