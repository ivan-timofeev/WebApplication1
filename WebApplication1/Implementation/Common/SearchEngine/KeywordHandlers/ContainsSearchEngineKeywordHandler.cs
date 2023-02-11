using System.Data.SqlTypes;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.Internal;
using WebApplication1.Common.SearchEngine.Abstractions;
using static WebApplication1.Common.SearchEngine.SearchEngineKeywordHandlerHelpers;

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

public interface ISearchEngineKeywordHandler2
{
    Expression<Func<T, bool>> HandleKeyword<T>(SearchEngineFilter.FilterToken filterToken);
}

public interface ISearchEngineKeywordHandlerFactory
{
    FilterTypeEnum FilterType { get; }
    ISearchEngineKeywordHandler2 CreateSearchEngineKeywordHandler();
}

public interface ISearchEngineKeywordHandlerFactoryFinder
{
    ISearchEngineKeywordHandlerFactory GetSearchEngineKeywordHandlerFactory(FilterTypeEnum filterType);
}

public class SearchEngineKeywordHandlerFactoryFinder
    : ISearchEngineKeywordHandlerFactoryFinder
{
    private readonly ISearchEngineKeywordHandlerFactory[] _searchEngineKeywordHandlerFactories;

    public SearchEngineKeywordHandlerFactoryFinder(
        IEnumerable<ISearchEngineKeywordHandlerFactory> searchEngineKeywordHandlerFactories)
    {
        _searchEngineKeywordHandlerFactories = searchEngineKeywordHandlerFactories.ToArray();
    }

    public ISearchEngineKeywordHandlerFactory GetSearchEngineKeywordHandlerFactory(FilterTypeEnum filterType)
    {
        return _searchEngineKeywordHandlerFactories.Single(x => x.FilterType == filterType);
    }
}



public static class SearchEngineKeywordHandlerHelpers
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

public class ContainsSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.Contains;

    public ISearchEngineKeywordHandler2 CreateSearchEngineKeywordHandler()
    {
        return new ContainsSearchEngineKeywordHandler2();
    }
}

public class ContainsSearchEngineKeywordHandler2 : ISearchEngineKeywordHandler2
{
    public Expression<Func<T, bool>> HandleKeyword<T>(
        SearchEngineFilter.FilterToken filterToken)
    {
        var entityType = typeof(T);
        var attributeType = GetAttributeType(entityType, filterToken.AttributeName);

        var parameter = Expression.Parameter(entityType, "type");
        var property = AccessToAttributeProperty(parameter, filterToken.AttributeName);
        var propertyNormalized = Expression.Call(property, GetStringToLowerMethodInfo());

        var filterValue = Expression.Constant(filterToken.AttributeValue, attributeType);
        var stringContains = Expression.Call(propertyNormalized, GetStringContainsMethodInfo(), filterValue);
        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(object)));

        var condition = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(nullCheck, stringContains),
            parameter);

        return condition;
    }

    private static MethodInfo GetStringContainsMethodInfo()
    {
        return typeof(string).GetMethod("Contains", new[] { typeof(string) })
               ?? throw new InvalidOperationException();
    }

    private static MethodInfo GetStringToLowerMethodInfo()
    {
        return typeof(string).GetMethod("ToLower", System.Type.EmptyTypes)
               ?? throw new InvalidOperationException();
    }
}

public class EqualsSearchEngineKeywordHandlerFactory
    : ISearchEngineKeywordHandlerFactory
{
    public FilterTypeEnum FilterType => FilterTypeEnum.Equals;
    
    private readonly ISearchEngineFilterAttributeParser _searchEngineFilterAttributeParser;

    public EqualsSearchEngineKeywordHandlerFactory(ISearchEngineFilterAttributeParser searchEngineFilterAttributeParser)
    {
        _searchEngineFilterAttributeParser = searchEngineFilterAttributeParser;
    }

    public ISearchEngineKeywordHandler2 CreateSearchEngineKeywordHandler()
    {
        return new EqualsSearchEngineKeywordHandler2(_searchEngineFilterAttributeParser);
    }
}

public class EqualsSearchEngineKeywordHandler2 : ISearchEngineKeywordHandler2
{
    private readonly ISearchEngineFilterAttributeParser _searchEngineFilterAttributeParser;

    public EqualsSearchEngineKeywordHandler2(ISearchEngineFilterAttributeParser searchEngineFilterAttributeParser)
    {
        _searchEngineFilterAttributeParser = searchEngineFilterAttributeParser;
    }

    public Expression<Func<T, bool>> HandleKeyword<T>(
        SearchEngineFilter.FilterToken filterToken)
    {
        var entityType = typeof(T);
        var attributeType = GetAttributeType(entityType, filterToken.AttributeName);

        var parameter = Expression.Parameter(entityType, "type");
        var property = AccessToAttributeProperty(parameter, filterToken.AttributeName);

        var parsedFilterValue =
            _searchEngineFilterAttributeParser.ParseAttribute(filterToken.AttributeValue, attributeType);
        var filterValue = Expression.Constant(parsedFilterValue, attributeType);

        if (!attributeType.IsNullableType())
        {
            return Expression.Lambda<Func<T, bool>>(body:
                Expression.Equal(property, filterValue),
                parameter);
        }
        
        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(object)));
        var equals = Expression.Equal(property, filterValue);

        return Expression.Lambda<Func<T, bool>>(body:
            Expression.AndAlso(nullCheck, equals),
            parameter);
    }

    private static MethodInfo GetStringContainsMethodInfo()
    {
        return typeof(string).GetMethod("Contains", new[] { typeof(string) })
               ?? throw new InvalidOperationException();
    }

    private static MethodInfo GetStringToLowerMethodInfo()
    {
        return typeof(string).GetMethod("ToLower", System.Type.EmptyTypes)
               ?? throw new InvalidOperationException();
    }
}

public interface ISearchEngineFilterAttributeParser
{
    // Вероятно лучше было завязаться на тип атрибута получаемый при помощи GetAttributeType(..)
    object ParseAttribute(string attributeValue, Type attributeType);
}

public class SearchEngineFilterAttributeParser : ISearchEngineFilterAttributeParser
{
    private readonly IEnumerable<IAttributeParserStrategy> _attributeParseStrategies;

    public SearchEngineFilterAttributeParser(IEnumerable<IAttributeParserStrategy> attributeParseStrategies)
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

public interface IAttributeParserStrategy
{
    Type AssignedType { get; }
    object ParseAttribute(string attributeValue);
}

public class AttributeParserStringStrategy : IAttributeParserStrategy
{
    public Type AssignedType => typeof(int);

    public object ParseAttribute(string attributeValue)
    {
        return !string.IsNullOrWhiteSpace(attributeValue)
            ? attributeValue
            : string.Empty;
    }
}

public class AttributeParserIntStrategy : IAttributeParserStrategy
{
    public Type AssignedType => typeof(int);

    public object ParseAttribute(string attributeValue)
    {
        return int.Parse(attributeValue);
    }
}

public class AttributeParserLongStrategy : IAttributeParserStrategy
{
    public Type AssignedType => typeof(long);

    public object ParseAttribute(string attributeValue)
    {
        return long.Parse(attributeValue);
    }
}

public class AttributeParserFloatStrategy : IAttributeParserStrategy
{
    public Type AssignedType => typeof(float);

    public object ParseAttribute(string attributeValue)
    {
        return float.Parse(attributeValue);
    }
}

public class AttributeParserDoubleStrategy : IAttributeParserStrategy
{
    public Type AssignedType => typeof(float);

    public object ParseAttribute(string attributeValue)
    {
        return float.Parse(attributeValue);
    }
}

public class AttributeParserDateTimeStrategy : IAttributeParserStrategy
{
    public Type AssignedType => typeof(DateTime);

    public object ParseAttribute(string attributeValue)
    {
        return DateTime.Parse(attributeValue, CultureInfo.InvariantCulture);
    }
}

public class AttributeParserGuidStrategy : IAttributeParserStrategy
{
    public Type AssignedType => typeof(Guid);

    public object ParseAttribute(string attributeValue)
    {
        return Guid.Parse(attributeValue);
    }
}
