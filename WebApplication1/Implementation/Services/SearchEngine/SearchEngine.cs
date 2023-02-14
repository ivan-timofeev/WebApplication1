using System.Linq.Expressions;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Common.SearchEngine;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine;

public class SearchEngine : ISearchEngine
{
    private readonly ISearchEngineFilterValidator _searchEngineFilterValidator;
    private readonly ISearchEngineKeywordHandlerFactoryProvider _searchEngineKeywordHandlerFactoryProvider;

    public SearchEngine(
        ISearchEngineFilterValidator searchEngineFilterValidator,
        ISearchEngineKeywordHandlerFactoryProvider searchEngineKeywordHandlerFactoryProvider)
    {
        _searchEngineFilterValidator = searchEngineFilterValidator
            ?? throw new ArgumentNullException(nameof(searchEngineFilterValidator));
        _searchEngineKeywordHandlerFactoryProvider = searchEngineKeywordHandlerFactoryProvider
            ?? throw new ArgumentNullException(nameof(searchEngineKeywordHandlerFactoryProvider));
    }

    /// <exception cref="SearchEngineFilterValidationException">An exception is thrown if an invalid filter was passed.</exception>
    public IQueryable<T> ExecuteEngine<T>(
        IQueryable<T> source,
        SearchEngineFilter? filter)
    {
        if (filter is null)
            return source;

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
        var searchEngineKeywordHandler = _searchEngineKeywordHandlerFactoryProvider
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
