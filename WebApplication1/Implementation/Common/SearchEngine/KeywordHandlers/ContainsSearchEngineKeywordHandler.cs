using System.Linq.Expressions;
using System.Reflection;
using WebApplication1.Common.SearchEngine.Abstractions;

namespace WebApplication1.Common.SearchEngine;

public class ContainsSearchEngineKeywordHandler : ISearchEngineKeywordHandler
{
    public string Keyword => "contains";

    public IQueryable<T> HandleKeyword<T>(IQueryable<T> source, string attributeName, object? attributeValue = null)
    {
        var attributeType = typeof(T)
            .GetProperties()
            .Single(x =>
                string.Equals(x.Name, attributeName, StringComparison.InvariantCultureIgnoreCase))
            .PropertyType;
        
        var parameterExp = Expression.Parameter(typeof(T), "type");
        var propertyExp = Expression.Property(parameterExp, attributeName);

        var propertyNormalizedExp = Expression.Call(propertyExp, GetToLowerMethod());
        
        var someValue = Expression.Constant(attributeValue, attributeType);
        var containsMethodExp = Expression.Call(propertyNormalizedExp, GetContainsMethod(), someValue);
        
        var nullCheck = Expression.NotEqual(propertyExp, Expression.Constant(null, typeof(object)));

        var condition = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(nullCheck, containsMethodExp), parameterExp);

        return source.Where(condition);
    }

    private static MethodInfo GetContainsMethod()
    {
        return typeof(string).GetMethod("Contains", new[] { typeof(string) })
               ?? throw new InvalidOperationException();
    }

    private static MethodInfo GetToLowerMethod()
    {
        return typeof(string).GetMethod("ToLower", System.Type.EmptyTypes)
            ?? throw new InvalidOperationException();
    }
}

public class ContainsSearchEngineKeywordHandler2
{
    public FilterTypeEnum Keyword => FilterTypeEnum.Contains;

    public Expression<Func<T, bool>> HandleKeyword<T>(
        IQueryable<T> source,
        SearchEngineFilter.FilterToken filterToken)
    {
        var entityType = typeof(T);
        var attributeType = GetAttributeType(entityType, filterToken.AttributeName);

        var parameter = Expression.Parameter(entityType, "type");
        var property = AccessToAttributeProperty(parameter, filterToken.AttributeName);
        var propertyNormalized = Expression.Call(property, GetToLowerMethod());

        var attributeValue = Expression.Constant(filterToken.AttributeValue, attributeType);
        var withContains = Expression.Call(propertyNormalized, GetContainsMethod(), attributeValue);
        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(object)));

        var condition = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(nullCheck, withContains),
            parameter);

        return condition;
    }

    private MemberExpression AccessToAttributeProperty(ParameterExpression parameterExpression, string pathToAttribute)
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

    private static Type GetAttributeType(Type entityType, string pathToAttribute)
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

    private static MethodInfo GetContainsMethod()
    {
        return typeof(string).GetMethod("Contains", new[] { typeof(string) })
               ?? throw new InvalidOperationException();
    }

    private static MethodInfo GetToLowerMethod()
    {
        return typeof(string).GetMethod("ToLower", System.Type.EmptyTypes)
               ?? throw new InvalidOperationException();
    }
}