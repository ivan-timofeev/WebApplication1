using AutoMapper;
using DeepEqual.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
        // Arrange
        var item1 = new DummyEntity(Field1: "1", Field2: "2");
        var item2 = new DummyEntity(Field1: "2", Field2: "3");
        var item3 = new DummyEntity(Field1: "3", Field2: "4");

        var data = new List<DummyEntity> { item1, item2, item3 };
        // ( Field1 = 1 || Field1 = 2 ) && Field2 = 2
        var filter = new SearchEngineFilterBuilder()
            .WithContains("Field1", "1", out var filterForA)
            .WithOrContains(filterForA, "Field1", "2", out filterForA)
            .WithOrContains(filterForA, "Field1", "3", out filterForA)
            .WithContains("Field2", "2", out _)
            .Build();


        // Act
        var source = data.AsQueryable();
        var searchEngine = new SearchEngine2(
            CreateMock<ISearchEngineFilterValidator>(out var validatorMock),
            Mock.Of<ISearchEngineKeywordHandlerFinder>());

        var filtered = searchEngine
            .ExecuteEngine(source, filter)
            .ToArray();


        // Assert
        Assert.Equal(2, filtered.Length);
        Assert.Contains(item1, filtered);
        Assert.Contains(item2, filtered);
        validatorMock.Verify(x => x.ValidateFilter(filter, typeof(DummyEntity)));
    }

    private static T CreateMock<T>(out Mock<T> mock)
        where T : class
    {
        mock = new Mock<T>();
        return mock.Object;
    }

    private ISearchEngine GetSearchEngine()
    {
        var services = new ServiceCollection();
        services.AddSearchEngine();
        var sp = services.BuildServiceProvider();
        
        return sp.GetRequiredService<ISearchEngine>();
    }

    record DummyEntity(string Field1, string Field2);
}