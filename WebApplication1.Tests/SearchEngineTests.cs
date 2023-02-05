using AutoMapper;
using DeepEqual.Syntax;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Common.SearchEngine;
using WebApplication1.Common.SearchEngine.Abstractions;
using WebApplication1.Common.SearchEngine.DependencyInjection;
using WebApplication1.Implementation.Helpers;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Tests;

public class SearchEngineTests
{
    [Fact]
    public void Test1()
    {
        var data = new List<MyTestClass>()
        {
            new MyTestClass()
            {
                Field1 = 1,
                Field2 = "Hello, world! 1",
                Field3 = DateTime.Now.AddDays(-1).TruncateHours()
            },
            new MyTestClass()
            {
                Field1 = 2,
                Field2 = "Hello, world! 2",
                Field3 = DateTime.Now.AddDays(-2).TruncateHours()
            },
            new MyTestClass()
            {
                Field1 = 2,
                Field2 = "Hello, world! 3",
                Field3 = DateTime.Now.AddDays(-3).TruncateHours()
            },
            new MyTestClass()
            {
                Field1 = null,
                Field2 = "Hello, world! 4",
                Field3 = DateTime.Now.AddDays(-4).TruncateHours()
            }
        };
        var query = data.AsQueryable();
        var searchEngine = GetSearchEngine();

        var result1 = searchEngine.ExecuteEngine(query, "field1 is 1 field2 contains world order-by field3")
            .ToArray();
        var result2 = searchEngine.ExecuteEngine(query, "field2 contains world order-by field3 asc")
            .ToArray();
    }

    private ISearchEngine GetSearchEngine()
    {
        var services = new ServiceCollection();
        services.AddSearchEngine();
        var sp = services.BuildServiceProvider();
        
        return sp.GetRequiredService<ISearchEngine>();
    }
    /*
     * if (SelectedOperator is StringOperators)
{
    MethodInfo method;

    var value = Expression.Constant(Value);

    switch ((StringOperators)SelectedOperator)
    {
        case StringOperators.Is:
            condition = Expression.Equal(property, value);
            break;

        case StringOperators.IsNot:
            condition = Expression.NotEqual(property, value);
            break;

        case StringOperators.StartsWith:
            method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            condition = Expression.Call(property, method, value);
            break;

        case StringOperators.Contains:
            method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            condition = Expression.Call(property, method, value);
            break;

        case StringOperators.EndsWith:
            method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            condition = Expression.Call(property, method, value);
            break;
    }
}
     */

    class MyTestClass
    {
        public int? Field1 { get; set; }
        public string Field2 { get; set; }
        public DateTime Field3 { get; set; }
    }
}