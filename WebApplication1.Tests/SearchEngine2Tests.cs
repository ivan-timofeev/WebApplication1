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
    public void ExecuteEngine_CorrectSimpleFilter_ShouldReturnFilteredCollection()
    {
        // Arrange
        var item1 = new DummyEntity(Field1: "1", Field2: "2");
        var item2 = new DummyEntity(Field1: "2", Field2: "3");
        var item3 = new DummyEntity(Field1: "3", Field2: "4");

        var data = new List<DummyEntity> { item1, item2, item3 };
        // ( Field1 = 1 || Field1 = 2 ) && Field2 = 2
        var filter = new SearchEngineFilterBuilder()
            .WithContains("Field1", "1", out var filterForField1)
            .WithOrContains(filterForField1, "Field1", "2", out filterForField1)
            .WithContains("Field2", "2", out var filterForField2)
            .WithOrContains(filterForField2, "Field2", "3", out filterForField2)
            .Build();


        // Act
        var source = data.AsQueryable();
        var searchEngine = CreateSearchEngine();

        var filtered = searchEngine
            .ExecuteEngine(source, filter)
            .ToArray();


        // Assert
        Assert.Equal(2, filtered.Length);
        Assert.Contains(item1, filtered);
        Assert.Contains(item2, filtered);
    }
    
    [Fact]
    public void ExecuteEngine_FilterWithEquals_ShouldReturnFilteredCollection()
    {
        // Arrange
        var item1 = new DummyEntityInt(Field1: 1, Field2: 2);
        var item2 = new DummyEntityInt(Field1: 3, Field2: 4);

        var data = new List<DummyEntityInt> { item1, item2 };
        // Field1 = 1 & Field2 = 2
        var filter = new SearchEngineFilterBuilder()
            .WithEquals(nameof(DummyEntity.Field1), "1", AttributeTypeEnum.Int, out _)
            .WithEquals(nameof(DummyEntity.Field2), "2", AttributeTypeEnum.Int, out _)
            .Build();


        // Act
        var source = data.AsQueryable();
        var searchEngine = CreateSearchEngine();

        var filtered = searchEngine
            .ExecuteEngine(source, filter)
            .ToArray();


        // Assert
        Assert.Single(filtered, item1);
    }
    
    [Fact]
    public void ExecuteEngine_FilterWithEqualsAndNullableFields_ShouldReturnFilteredCollection()
    {
        // Arrange
        var item1 = new DummyEntityNullableInt(Field1: 1, Field2: 2);
        var item2 = new DummyEntityNullableInt(Field1: 3, Field2: null);

        var data = new List<DummyEntityNullableInt> { item1, item2 };
        // Field1 = 1 & Field2 = 2
        var filter = new SearchEngineFilterBuilder()
            .WithEquals(nameof(DummyEntity.Field1), "1", AttributeTypeEnum.Int, out _)
            .WithEquals(nameof(DummyEntity.Field2), "2", AttributeTypeEnum.Int, out _)
            .Build();


        // Act
        var source = data.AsQueryable();
        var searchEngine = CreateSearchEngine();

        var filtered = searchEngine
            .ExecuteEngine(source, filter)
            .ToArray();


        // Assert
        Assert.Single(filtered, item1);
    }
    
    [Fact]
    public void ExecuteEngine_CorrectSimpleFilterWithLevelTwoAttributeAccess_ShouldReturnFilteredCollection()
    {
        // Arrange
        var item1 = new ComplexDummyEntity(Field1: "1", Field2: new SubDummyEntity(Field3: "2"));
        var item2 = new ComplexDummyEntity(Field1: "3", Field2: new SubDummyEntity(Field3: "4"));
        var item3 = new ComplexDummyEntity(Field1: "5", Field2: new SubDummyEntity(Field3: "6"));

        var data = new List<ComplexDummyEntity> { item1, item2, item3 };
        // Field1 = 1 && Field2.Field3 = 2
        var filter = new SearchEngineFilterBuilder()
            .WithContains("Field1", "1", out _)
            .WithContains("Field2.Field3", "2", out _)
            .Build();


        // Act
        var source = data.AsQueryable();
        var searchEngine = CreateSearchEngine();

        var filtered = searchEngine
            .ExecuteEngine(source, filter)
            .ToArray();


        // Assert
        Assert.Single(filtered, item1);
    }

    [Fact]
    public void AddSearchEngine2_CreateCorrectSearchEngineInstance_ServicesShouldContainsAllRequiredParts()
    {
        // Arrange & Act
        var services = new ServiceCollection();
        services.AddSearchEngine2();
        services.BuildServiceProvider();


        // Assert
        /* Common part */
        AssertRequiredServiceAreRegistered(services, typeof(SearchEngine2));
        AssertRequiredServiceAreRegistered(services, typeof(SearchEngineFilterValidator));
        AssertRequiredServiceAreRegistered(services, typeof(SearchEngineKeywordHandlerFactoryFinder));
        AssertRequiredServiceAreRegistered(services, typeof(SearchEngineFilterAttributeParser));
        /* SearchEngineKeywordHandlers part */
        AssertRequiredServiceAreRegistered(services, typeof(ContainsSearchEngineKeywordHandler2));
        AssertRequiredServiceAreRegistered(services, typeof(EqualsSearchEngineKeywordHandler2));
        /* SearchEngineKeywordHandlerFactories part */
        AssertRequiredServiceAreRegistered(services, typeof(ContainsSearchEngineKeywordHandlerFactory));
        AssertRequiredServiceAreRegistered(services, typeof(EqualsSearchEngineKeywordHandlerFactory));
    }

    private void AssertRequiredServiceAreRegistered(ServiceCollection services, Type requiredServiceType)
    {
        Assert.Contains(services, x => x.ImplementationType == requiredServiceType);
    }

    private ISearchEngine2 CreateSearchEngine()
    {
        return new ServiceCollection()
            .AddSearchEngine2()
            .BuildServiceProvider()
            .GetRequiredService<ISearchEngine2>();
    }

    record DummyEntity(string Field1, string Field2);
    record DummyEntityInt(int Field1, int Field2);
    record DummyEntityNullableInt(int? Field1, int? Field2);
    record ComplexDummyEntity(string Field1, SubDummyEntity Field2);
    record SubDummyEntity(string Field3);
}