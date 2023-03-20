using System.Collections;
using AutoMapper;
using DeepEqual.Syntax;
using WebApplication1.Common.Configuration;
using WebApplication1.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels.SearchEngine;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Tests;

public class SearchEngineFilterMappingTests
{
    [Theory]
    [ClassData(typeof(FromFilterVmToFilterTestData))]
    public void Map_FromSearchEngineFilterVm_ToSearchEngineFilter(
        SearchEngineFilter expectedFilter,
        SearchEngineFilterVm filterVm)
    {
        // Arrange
        var mapper = GetMapper();


        // Act
        var actualFilter = mapper.Map<SearchEngineFilter>(filterVm);


        // Assert
        actualFilter.WithDeepEqual(expectedFilter)
            .IgnoreUnmatchedProperties()
            .Assert();
    }
    
    [Theory]
    [ClassData(typeof(FromJsonToFilterTestData))]
    public void Map_FromJson_ToSearchEngineFilter(
        SearchEngineFilter expectedFilter,
        string filterJson)
    {
        // Arrange
        var mapper = GetMapper();


        // Act
        var filterVm = JsonSerializer.Deserialize<SearchEngineFilterVm>(filterJson.Replace('\'', '\"'));
        var actualFilter = mapper.Map<SearchEngineFilter>(filterVm);


        // Assert
        actualFilter.WithDeepEqual(expectedFilter)
            .IgnoreUnmatchedProperties()
            .Assert();
    }

    private Mapper GetMapper()
    {
        return new Mapper(AutomapperConfiguration.GetAutomapperConfiguration());
    }
}

public class FromFilterVmToFilterTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Foo contains 1 && Bar contains 2
        yield return new object[]
        {
            new SearchEngineFilterBuilder()
                .WithContains("Foo", "1", out _)
                .WithContains("Bar", "2", out _)
                .Build(),
            new SearchEngineFilterVm()
            {
                FilterTokenGroups = new[]
                {
                    new FilterTokenVm
                    {
                        FilterType = FilterTypeEnum.Contains,
                        AttributeName = "Foo",
                        AttributeType = AttributeTypeEnum.Text,
                        AttributeValue = "1"
                    },
                    new FilterTokenVm
                    {
                        FilterType = FilterTypeEnum.Contains,
                        AttributeName = "Bar",
                        AttributeType = AttributeTypeEnum.Text,
                        AttributeValue = "2"
                    }
                }
            }
        };
        
        // ( Foo contains 1 || Foo contains 2 ) && Bar contains 2
        yield return new object[]
        {
            new SearchEngineFilterBuilder()
                .WithContains("Foo", "1", out var fooFilter)
                .WithOrContains(fooFilter, "Foo", "2", out _)
                .WithContains("Bar", "2", out _)
                .Build(),
            new SearchEngineFilterVm()
            {
                FilterTokenGroups = new FilterTokenBaseVm[]
                {
                    new FilterTokenGroupVm
                    {
                        FilterTokens = new []
                        {
                            new FilterTokenVm
                            {
                                FilterType = FilterTypeEnum.Contains,
                                AttributeName = "Foo",
                                AttributeType = AttributeTypeEnum.Text,
                                AttributeValue = "1"
                            },
                            new FilterTokenVm
                            {
                                FilterType = FilterTypeEnum.Contains,
                                AttributeName = "Foo",
                                AttributeType = AttributeTypeEnum.Text,
                                AttributeValue = "2"
                            }
                        },
                        Operation = FilterTokenGroupOperationEnum.Or
                    },
                    new FilterTokenVm
                    {
                        FilterType = FilterTypeEnum.Contains,
                        AttributeName = "Bar",
                        AttributeType = AttributeTypeEnum.Text,
                        AttributeValue = "2"
                    }
                }
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class FromJsonToFilterTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // ( Foo contains 1 || Foo contains 2 ) && Bar contains 2
        yield return new object[]
        {
            // Expected output:
            new SearchEngineFilterBuilder()
                .WithContains("Foo", "1", out var fooFilter)
                .WithOrContains(fooFilter, "Foo", "2", out _)
                .WithContains("Bar", "2", out _)
                .Build(),
            // Input:
            @"{
                'FilterTokenGroups': [
                  {
                    '$type': 'group',
                    'FilterTokens': [
                      {
                        '$type': 'token',
                        'AttributeName': 'Foo',
                        'AttributeValue': '1',
                        'AttributeType': 0,
                        'FilterType': 1
                      },
                      {
                        '$type': 'token',
                        'AttributeName': 'Foo',
                        'AttributeValue': '2',
                        'AttributeType': 0,
                        'FilterType': 1
                      }
                    ],
                    'Operation': 1
                  },
                  {
                    '$type': 'token',
                    'AttributeName': 'Bar',
                    'AttributeValue': '2',
                    'AttributeType': 0,
                    'FilterType': 1
                  }
                ]
              }"
        };
        
        // Foo contains 1 && Bar contains 2
        yield return new object[]
        {
            // Expected output:
            new SearchEngineFilterBuilder()
                .WithContains("Foo", "1", out _)
                .WithContains("Bar", "2", out _)
                .Build(),
            // Input:
            @"{
                'FilterTokenGroups': [
                  {
                    '$type': 'token',
                    'AttributeName': 'Foo',
                    'AttributeValue': '1',
                    'AttributeType': 0,
                    'FilterType': 1
                  },
                  {
                    '$type': 'token',
                    'AttributeName': 'Bar',
                    'AttributeValue': '2',
                    'AttributeType': 0,
                    'FilterType': 1
                  }
                ]
              }"
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
