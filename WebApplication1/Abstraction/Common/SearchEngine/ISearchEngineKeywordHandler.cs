using System.Linq.Expressions;
using WebApplication1.Common.SearchEngine.Models;


namespace WebApplication1.Abstraction.Common.SearchEngine;

public interface ISearchEngineKeywordHandler
{
    Expression<Func<T, bool>> HandleKeyword<T>(SearchEngineFilter.FilterToken filterToken);
}
