using System.Linq.Expressions;

namespace WebApplication1.Helpers.SearchEngine.Abstractions;

public class IsSearchEngineKeywordHandler : ISearchEngineKeywordHandler
{
    public string Keyword => "is";

    public IQueryable<T> HandleKeyword<T>(
        IQueryable<T> source,
        string attributeName,
        object attributeValue /* from search string */)
    {
        var attributeType = typeof(T)
            .GetProperties()
            .Single(x =>
                string.Equals(x.Name, attributeName, StringComparison.InvariantCultureIgnoreCase))
            .PropertyType;
        
        var parameterExpression = Expression.Parameter(typeof(T));
        var convertedAttributeValue = ConvertFromText(attributeValue, attributeType);
        
        var condition = Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
                Expression.Property(parameterExpression, attributeName),
                Expression.Constant(convertedAttributeValue, attributeType)
            ),
            parameterExpression
        );
        
        return source.Where(condition);
    }
    
    // TODO сделать адекватно
    static object ConvertFromText(object value, Type destinationType)
    {
        var text = value.ToString()
                   ?? throw new NullReferenceException("SOMETHING WENT WRONG SUKA");
        
        if (destinationType == typeof(int))
        {
            return int.Parse(text);
        }

        if (destinationType == typeof(int?))
        {
            if (int.TryParse(text, out var parsed))
                return parsed;
            return null;
        }

        if (destinationType == typeof(DateTime))
        {
            return DateTime.Parse(text);
        }

        return text;
    }
}