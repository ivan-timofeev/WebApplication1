using System.Linq.Expressions;
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
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) })
                     ?? throw new InvalidOperationException();
        
        var someValue = Expression.Constant(attributeValue, attributeType);
        var containsMethodExp = Expression.Call(propertyExp, method, someValue);
        
        var nullCheck = Expression.NotEqual(propertyExp, Expression.Constant(null, typeof(object)));

        var condition = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(nullCheck, containsMethodExp), parameterExp);

        return source.Where(condition);
    }
}