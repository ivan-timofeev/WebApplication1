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

public class SearchEngine2Tests
{
    [Fact]
    public void Test1()
    {
        var data = new List<Data>()
        {
            new Data
            {
                A = "1",
                B = "2"
            },
            new Data
            {
                A = "2",
                B = "4"
            }
        };

        var filterBuilder = new SearchEngineFilterBuilder();
        
        // ( A = 1 OR A = 2 ) AND B = 2
        var filter = filterBuilder
            .WithContains("A", "1", out var filterForA)
            .WithOrContains(filterForA, "A", "2", out filterForA)
            .WithOrContains(filterForA, "A", "3", out filterForA)
            .WithContains("B", "2", out _)
            .Build();
        
        
        var source = data.AsQueryable();
        var searchEngine = new SearchEngine2(null);

        var filtered = searchEngine
            .ExecuteEngine(source, filter)
            .ToArray();
    }

    private ISearchEngine GetSearchEngine()
    {
        var services = new ServiceCollection();
        services.AddSearchEngine();
        var sp = services.BuildServiceProvider();
        
        return sp.GetRequiredService<ISearchEngine>();
    }

    class Data
    {
        public string A { get; set; }
        public string B { get; set; }
    }
}