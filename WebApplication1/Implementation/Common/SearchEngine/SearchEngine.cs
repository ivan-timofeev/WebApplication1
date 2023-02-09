using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoMapper.Internal;
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
        var filterType = filterToken.FilterType;
        var attributeType = filterToken.AttributeType;
        
        if (filterType is FilterTypeEnum.LessThan or FilterTypeEnum.MoreThan)
        {
            if (attributeType is not (AttributeTypeEnum.Float or AttributeTypeEnum.Int or AttributeTypeEnum.DateTime))
            {
                throw new SearchEngineFilterValidationException(message:
                    string.Format("Filter type \"{0}\" cannot be used with value type \"{1}\".",
                        filterType,
                        attributeType));
            }
        }

        if (filterType is FilterTypeEnum.Contains or FilterTypeEnum.StartWith)
        {
            if (attributeType is not (AttributeTypeEnum.String))
            {
                throw new SearchEngineFilterValidationException(message:
                    string.Format("Filter type \"{0}\" cannot be used with value type \"{1}\".",
                        filterType,
                        attributeType));
            }
        }
    }
    
    private static void ValidateFilterTokenByType(SearchEngineFilter.FilterToken filterToken, Type entityType)
    {
        var attributeType = GetAttributeType(filterToken.AttributeName, entityType);

        if (attributeType is null)
        {
            throw new SearchEngineFilterValidationException(message:
                string.Format("Entity \"{0}\" does not contain attribute with name \"{1}\".",
                    entityType.Name,
                    filterToken.AttributeName));
        }

        var attributeTypeName = GetAttributeTypeName(attributeType);
        var filterAttributeTypeName = filterToken.AttributeType.ToString().ToLower();

        if (!filterAttributeTypeName.Contains(attributeTypeName))
        {
            throw new SearchEngineFilterValidationException(message:
                string.Format("Provided attribute type \"{0}\" does not match actual attribute type \"{1}\".",
                    filterAttributeTypeName,
                    attributeTypeName));
        }
    }

    private static Type? GetAttributeType(string attributeName, Type entityType)
    {
        var split = attributeName.ToLower().Split(".");
        var buffer = entityType;

        foreach (var attributePathPart in split)
        {
            buffer = buffer.GetProperties()
                .First(x => attributePathPart.ToLower().Contains(x.Name.ToLower()))
                .PropertyType;
        }

        return buffer;
    }

    private static string GetAttributeTypeName(Type attributeType)
    {
        var attributeTypeName = attributeType.IsNullableType()
            ? attributeType.GetGenericArguments()[0].Name
            : attributeType.Name;
        
        var typeName = attributeTypeName
            .ToLower()
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

public interface ISearchEngine2
{
    public IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, SearchEngineFilter filter);
}

public class SearchEngine2 : ISearchEngine2
{
    private readonly ISearchEngineFilterValidator _searchEngineFilterValidator;
    private readonly ISearchEngineKeywordHandlerFactoryFinder _searchEngineKeywordHandlerFactoryFinder;

    public SearchEngine2(
        ISearchEngineFilterValidator searchEngineFilterValidator,
        ISearchEngineKeywordHandlerFactoryFinder searchEngineKeywordHandlerFactoryFinder)
    {
        _searchEngineFilterValidator = searchEngineFilterValidator
            ?? throw new ArgumentNullException(nameof(searchEngineFilterValidator));
        _searchEngineKeywordHandlerFactoryFinder = searchEngineKeywordHandlerFactoryFinder
            ?? throw new ArgumentNullException(nameof(searchEngineKeywordHandlerFactoryFinder));
    }

    /// <exception cref="SearchEngineFilterValidationException">An exception is thrown if an invalid filter was passed.</exception>
    public IQueryable<T> ExecuteEngine<T>(
        IQueryable<T> source,
        SearchEngineFilter filter)
    {
        _searchEngineFilterValidator.ValidateFilter(filter, typeof(T));
        var condition = SynthesizeCondition<T>(filter.FilterTokenGroups.ToArray(), filter.Operation);
        var filteredSource = source.Where(condition);
        return filteredSource;
    }

    private Expression<Func<T, bool>> SynthesizeCondition<T>(
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
                    ? HandleKeyword<T>(filterToken)
                    : CombineConditions(condition, HandleKeyword<T>(filterToken), groupOperation);
            }
            else if (filterElement is SearchEngineFilter.FilterTokenGroup filterTokenGroup)
            {
                condition = SynthesizeCondition(
                    filterTokenGroup.FilterTokens.ToArray(),
                    filterTokenGroup.Operation,
                    condition);
            }
        }

        if (condition is null)
            throw new Exception("Что то явно пошло не так. " + 
                "Возможно в этой жизни ты свернул не на ту тропу. Подумай над этим.");

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

    private Expression<Func<T, bool>> HandleKeyword<T>(SearchEngineFilter.FilterToken filterToken)
    {
        var searchEngineKeywordHandler = _searchEngineKeywordHandlerFactoryFinder
            .GetSearchEngineKeywordHandlerFactory(filterToken.FilterType)
            .CreateSearchEngineKeywordHandler();

        return searchEngineKeywordHandler.HandleKeyword<T>(filterToken);
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

