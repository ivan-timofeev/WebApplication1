using System.Linq.Expressions;

namespace WebApplication1.Common.SearchEngine;

public static class ExpressionHelpers
{
    public static MemberExpression AccessToAttributeProperty(ParameterExpression parameterExpression, string pathToAttribute)
    {
        var split = pathToAttribute.Split('.');

        if (split.Length < 2)
        {
            return Expression.Property(parameterExpression, pathToAttribute);
        }
        
        Expression propertyExpression = parameterExpression;

        foreach (var property in split)
        {
            propertyExpression = Expression.PropertyOrField(propertyExpression, property);
        }

        return (MemberExpression)propertyExpression;
    }

    public static Type GetAttributeType(Type entityType, string pathToAttribute)
    {
        var split = pathToAttribute.ToLower().Split(".");
        var buffer = entityType;

        foreach (var attributePathPart in split)
        {
            buffer = buffer.GetProperties()
                .First(x => attributePathPart.ToLower().Contains(x.Name.ToLower()))
                .PropertyType;
        }

        return buffer;
    }
}
