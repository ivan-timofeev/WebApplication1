using System.Linq.Expressions;
using AutoMapper.Internal;
using WebApplication1.Abstractions.Services.SearchEngine;
using WebApplication1.Abstractions.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.KeywordHandlers;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine.KeywordHandlers;

public class GreaterThanSearchEngineKeywordHandler : ISearchEngineKeywordHandler
{
    private readonly ISearchEngineFilterAttributeParser _searchEngineFilterAttributeParser;

    public GreaterThanSearchEngineKeywordHandler(ISearchEngineFilterAttributeParser searchEngineFilterAttributeParser)
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
                Expression.GreaterThan(property, filterValue),
                parameter);
        }

        var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(object)));
        var greaterThan = Expression.GreaterThan(property, filterValue);

        return Expression.Lambda<Func<T, bool>>(body:
            Expression.AndAlso(nullCheck, greaterThan),
            parameter);
    }
}
