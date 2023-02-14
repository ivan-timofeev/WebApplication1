using System.Linq.Expressions;
using AutoMapper.Internal;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Common.SearchEngine;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class EqualsSearchEngineKeywordHandler : ISearchEngineKeywordHandler
{
    private readonly ISearchEngineFilterAttributeParser _searchEngineFilterAttributeParser;

    public EqualsSearchEngineKeywordHandler(ISearchEngineFilterAttributeParser searchEngineFilterAttributeParser)
    {
        _searchEngineFilterAttributeParser = searchEngineFilterAttributeParser;
    }

    public Expression<Func<T, bool>> HandleKeyword<T>(
        SearchEngineFilter.FilterToken filterToken)
    {
        var entityType = typeof(T);
        var attributeType = ExpressionHelpers.GetAttributeType(entityType, filterToken.AttributeName);

        var parameter = Expression.Parameter(entityType, "type");
        var property = ExpressionHelpers.AccessToAttributeProperty(parameter, filterToken.AttributeName);

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
}
