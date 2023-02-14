
using System.Linq.Expressions;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstraction.Services.SearchEngine;

public interface ISearchEngineKeywordHandler
{
    Expression<Func<T, bool>> HandleKeyword<T>(SearchEngineFilter.FilterToken filterToken);
}
