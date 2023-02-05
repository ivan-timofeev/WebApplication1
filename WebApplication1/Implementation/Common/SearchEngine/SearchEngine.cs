using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using WebApplication1.Data.Repositories;
using WebApplication1.Implementation.ViewModels;

namespace WebApplication1.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.Abstractions;

public class SearchEngine : ISearchEngine
{
    private readonly ISearchEngineQueryParser _searchEngineQueryParser;

    public SearchEngine(ISearchEngineQueryParser searchEngineQueryParser)
    {
        _searchEngineQueryParser = searchEngineQueryParser;
    }
    
    public IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, string searchQuery)
    {
        var parsedQuery = _searchEngineQueryParser.ParseSearchQuery(searchQuery);

        while (parsedQuery.TryDequeue(out var queryUnit))
        {
            var attributeName = queryUnit.AttributeName;
            var attributeValue = queryUnit.AttributeValue;

            source = queryUnit.Handler.HandleKeyword(source, attributeName, attributeValue);
        }

        return source;
    }
}

public interface ISearchEngineFilterValidator
{
    /// <exception cref="SearchEngineFilterValidationException">An exception is thrown if an invalid filter was passed.</exception>
    void ValidateFilter(SearchEngineFilter filter, Type entityType);
}

public class SearchEngineFilterValidator : ISearchEngineFilterValidator
{
    public void ValidateFilter(SearchEngineFilter filter, Type entityType)
    {
        var filterTokens = CollectFilterTokens(filter);
        BaseValidation(filterTokens);
        TypeValidation(filterTokens, entityType);
    }

    private void BaseValidation(SearchEngineFilter.FilterToken[] filterTokens)
    {
        foreach (var item in filterTokens)
        {
            ValidateFilterToken(item);
        }
    }

    private void TypeValidation(SearchEngineFilter.FilterToken[] filterTokens, Type entityType)
    {
        foreach (var item in filterTokens)
        {
            ValidateFilterTokenByType(item, entityType);
        }
    }

    private static void ValidateFilterToken(SearchEngineFilter.FilterToken filterToken)
    {
        if (filterToken.FilterType is FilterTypeEnum.LessThan or FilterTypeEnum.MoreThan)
        {
            if (filterToken.AttributeType is not (AttributeTypeEnum.Float or AttributeTypeEnum.Int or AttributeTypeEnum.DateTime))
            {
                throw new SearchEngineFilterValidationException(message:
                    string.Format("Filter type \"{0}\" cannot be used with value type \"{1}\".",
                        filterToken.FilterType,
                        filterToken.AttributeType));
            }
        }
    }
    
    private static void ValidateFilterTokenByType(SearchEngineFilter.FilterToken filterToken, Type entityType)
    {
        var attribute = entityType.GetProperties()
            .FirstOrDefault(x => filterToken.VariableName.ToLower().Contains(x.Name.ToLower()));

        if (attribute is null)
        {
            throw new SearchEngineFilterValidationException(message:
                string.Format("Entity type \"{0}\" does not contain field \"{1}\".",
                    entityType.Name,
                    filterToken.VariableName));
        }

        var attributeTypeName = GetAttributeTypeName(attribute);
        var filterAttributeTypeName = filterToken.AttributeType.ToString().ToLower();

        if (!filterAttributeTypeName.Contains(attributeTypeName))
        {
            throw new SearchEngineFilterValidationException(message:
                string.Format("Provided attribute type \"{0}\" does not match actual attribute type \"{1}\".",
                    filterAttributeTypeName,
                    attributeTypeName));
        }
    }

    private static string GetAttributeTypeName(PropertyInfo propertyInfo)
    {
        var typeName = propertyInfo.PropertyType.Name.ToLower()
            .Replace("single", "float")
            .Replace("double", "float");
        return Regex.Replace(typeName, @"[\d-]", string.Empty);
    }

    private static SearchEngineFilter.FilterToken[] CollectFilterTokens(SearchEngineFilter filter)
    {
        var tokens = new List<SearchEngineFilter.FilterToken>();

        foreach (var item in filter.FilterTokenGroups)
        {
            switch (item)
            {
                case SearchEngineFilter.FilterTokenGroup filterTokenGroup:
                    tokens.AddRange(CollectFilterTokens(filterTokenGroup));
                    break;
                case SearchEngineFilter.FilterToken filterToken:
                    tokens.Add(filterToken);
                    break;
            }
        }

        return tokens.ToArray();
    }

    private static List<SearchEngineFilter.FilterToken> CollectFilterTokens(SearchEngineFilter.FilterTokenGroup input)
    {
        var tokens = new List<SearchEngineFilter.FilterToken>();
        
        foreach (var item in input.FilterTokens)
        {
            switch (item)
            {
                case SearchEngineFilter.FilterTokenGroup filterTokenGroup:
                    tokens.AddRange(CollectFilterTokens(filterTokenGroup));
                    break;
                case SearchEngineFilter.FilterToken filterToken:
                    tokens.Add(filterToken);
                    break;
            }
        }

        return tokens;
    }
}

public class SearchEngineFilterValidationException : Exception, IErrorVmProvider
{
    public SearchEngineFilterValidationException(string message)
        : base(message)
    {
        
    }
    
    public ErrorVm GetErrorVm()
    {
        throw new NotImplementedException();
    }

    public int GetHttpStatusCode()
    {
        throw new NotImplementedException();
    }
}

public class SearchEngine2 // : ISearchEngine
{
    private readonly ISearchEngineFilterValidator _searchEngineFilterValidator;
    private readonly ISearchEngineKeywordHandlerFinder _searchEngineKeywordHandlerFinder;

    public SearchEngine2(
        ISearchEngineFilterValidator searchEngineFilterValidator,
        ISearchEngineKeywordHandlerFinder searchEngineKeywordHandlerFinder)
    {
        _searchEngineFilterValidator = searchEngineFilterValidator
            ?? throw new ArgumentNullException(nameof(searchEngineFilterValidator));
        _searchEngineKeywordHandlerFinder = searchEngineKeywordHandlerFinder
            ?? throw new ArgumentNullException(nameof(searchEngineKeywordHandlerFinder));
    }

    /// <exception cref="SearchEngineFilterValidationException">An exception is thrown if an invalid filter was passed.</exception>
    public IQueryable<T> ExecuteEngine<T>(
        IQueryable<T> source,
        SearchEngineFilter filter)
    {
        _searchEngineFilterValidator.ValidateFilter(filter, typeof(T));
        var condition = SynthesizeCondition(source, filter.FilterTokenGroups.ToArray(), filter.Operation);
        var filteredSource = source.Where(condition);
        return filteredSource;
    }

    private Expression<Func<T, bool>> SynthesizeCondition<T>(
        IQueryable<T> source,
        IFilterToken[] filterTokens,
        FilterTokenGroupOperationEnum groupOperation,
        Expression<Func<T, bool>>? condition = null)
    {
        if (filterTokens == null)
            throw new ArgumentNullException(nameof(filterTokens));

        foreach (var filterElement in filterTokens)
        {
            if (filterElement is SearchEngineFilter.FilterToken filterToken)
            {
                condition = condition is null
                    ? HandleKeyword(source, filterToken)
                    : CombineConditions(condition, HandleKeyword(source, filterToken), groupOperation);
            }
            else if (filterElement is SearchEngineFilter.FilterTokenGroup filterTokenGroup)
            {
                condition = SynthesizeCondition(
                    source,
                    filterTokenGroup.FilterTokens.ToArray(),
                    filterTokenGroup.Operation,
                    condition);
            }
        }

        return condition;
    }

    private Expression<Func<T, bool>> CombineConditions<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        FilterTokenGroupOperationEnum expressionType)
    {
        switch (expressionType)
        {
            case FilterTokenGroupOperationEnum.And:
                return AndAlso(left, right);
            case FilterTokenGroupOperationEnum.Or:
                return OrElse(left, right);
            default:
                throw new Exception();
        }
        
    }

    private Expression<Func<T, bool>> HandleKeyword<T>(IQueryable<T> source, SearchEngineFilter.FilterToken filterToken)
    {
        var testHandler = new ContainsSearchEngineKeywordHandler2();
        return testHandler.HandleKeyword(source, filterToken.VariableName, filterToken.AttributeValue);
    }

    private static Expression<Func<T, bool>> AndAlso<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof (T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(left, right), parameter);
    }
    
    private static Expression<Func<T, bool>> OrElse<T>(
        Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof (T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(left, right), parameter);
    }

    private class ReplaceExpressionVisitor
        : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
    
}

