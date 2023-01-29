using System.Linq.Expressions;

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

public class SearchEngine2 // : ISearchEngine
{
    private readonly ISearchEngineKeywordHandlerFinder _searchEngineKeywordHandlerFinder;

    public SearchEngine2(ISearchEngineKeywordHandlerFinder searchEngineKeywordHandlerFinder)
    {
        _searchEngineKeywordHandlerFinder = searchEngineKeywordHandlerFinder;
    }
    
    public IQueryable<T> ExecuteEngine<T>(IQueryable<T> source, SearchEngineFilter filter)
    {
        // TODO add filter validation
        var condition = SynthesizeCondition(source, filter.FilterTokenGroups.ToArray(), filter.Operation);
        return source.Where(condition);
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
        return testHandler.HandleKeyword(source, filterToken.VariableName, filterToken.VariableValue);
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

