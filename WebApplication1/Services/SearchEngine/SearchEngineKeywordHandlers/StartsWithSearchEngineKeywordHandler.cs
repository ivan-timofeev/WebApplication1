using System.Linq.Expressions;
using System.Reflection;
using WebApplication1.Abstractions.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class StartsWithSearchEngineKeywordHandler : ISearchEngineKeywordHandler
{
    public Expression<Func<T, bool>> HandleKeyword<T>(
        SearchEngineFilter.FilterToken filterToken)
    {
        var entityType = typeof(T);
        var attributeType = ExpressionHelpers.GetAttributeType(entityType, filterToken.AttributeName);

        var parameter = Expression.Parameter(entityType, "type");
        var property = ExpressionHelpers.AccessToAttributeProperty(parameter, filterToken.AttributeName);
        var propertyNormalized = Expression.Call(property, GetStringToLowerMethodInfo());

        var filterValue = Expression.Constant(filterToken.AttributeValue.ToLower(), attributeType);
        var stringStartsWith = Expression.Call(propertyNormalized, GetStringStartsWithMethodInfo(), filterValue);
        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(object)));

        var condition = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(nullCheck, stringStartsWith),
            parameter);

        return condition;
    }

    private static MethodInfo GetStringStartsWithMethodInfo()
    {
        return typeof(string).GetMethod("StartsWith", new[] { typeof(string) })
               ?? throw new InvalidOperationException();
    }

    private static MethodInfo GetStringToLowerMethodInfo()
    {
        return typeof(string).GetMethod("ToLower", System.Type.EmptyTypes)
               ?? throw new InvalidOperationException();
    }
}
