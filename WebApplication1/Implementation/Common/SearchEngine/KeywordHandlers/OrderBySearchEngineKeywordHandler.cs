using System.Linq.Expressions;
using WebApplication1.Common.SearchEngine.Abstractions;

namespace WebApplication1.Common.SearchEngine;

interface ICanBeUnary
{
    
}

public class OrderBySearchEngineKeywordHandler : ISearchEngineKeywordHandler, ICanBeUnary
{
    public string Keyword => "order-by";

    public IQueryable<T> HandleKeyword<T>(IQueryable<T> source, string attributeName, object? attributeValue)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var conversion = Expression.Convert(Expression.Property(parameter, attributeName), typeof(object));   //important to use the Expression.Convert
        var selector = Expression.Lambda<Func<T, object>>(conversion, parameter);
        
        return string.Equals(attributeValue?.ToString() ?? "asc", "asc", StringComparison.InvariantCultureIgnoreCase)
            ? source.OrderBy(selector)
            : source.OrderByDescending(selector);
    }
}