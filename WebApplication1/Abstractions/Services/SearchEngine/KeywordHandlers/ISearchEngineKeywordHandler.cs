using System.Linq.Expressions;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstractions.Services.SearchEngine.KeywordHandlers;

public interface ISearchEngineKeywordHandler
{
    Expression<Func<T, bool>> HandleKeyword<T>(SearchEngineFilter.FilterToken filterToken);
}
