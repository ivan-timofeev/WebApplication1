using AutoBogus;
using AutoMapper;
using DeepEqual.Syntax;
using WebApplication1.Common.SearchEngine;
using WebApplication1.Implementation.Helpers;
using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using Xunit.Sdk;

namespace WebApplication1.Tests;

public class SearchEngineFilterBuilderTests
{
    [Fact]
    public void SearchEngineFilterBuilder_TestBuild_ShouldReturnExpectedFilter()
    {
        // Arrange
        var expectedVariableOneName = Random.Shared.Next().ToString();
        var expectedVariableTwoName = Random.Shared.Next().ToString();
        var expectedVariableOneFirstValue = Random.Shared.Next().ToString();
        var expectedVariableOneSecondValue = Random.Shared.Next().ToString();
        var expectedVariableTwoFirstValue = Random.Shared.Next().ToString();
        var expectedVariableTwoSecondValue = Random.Shared.Next().ToString();

        var expectedFilter = new SearchEngineFilter()
        {
            FilterTokenGroups = new List<IFilterToken>()
            {
                new SearchEngineFilter.FilterTokenGroup
                {
                    FilterTokens = new List<IFilterToken>
                    {
                        new SearchEngineFilter.FilterToken
                        {
                            VariableName = expectedVariableOneName,
                            VariableValue = expectedVariableOneFirstValue,
                            VariableType = VariableTypeEnum.String,
                            FilterType = FilterTypeEnum.Equals
                        },
                        new SearchEngineFilter.FilterToken
                        {
                            VariableName = expectedVariableOneName,
                            VariableValue = expectedVariableOneSecondValue,
                            VariableType = VariableTypeEnum.String,
                            FilterType = FilterTypeEnum.Equals
                        }
                    },
                    Operation = FilterTokenGroupOperationEnum.Or
                },
                new SearchEngineFilter.FilterTokenGroup
                {
                    FilterTokens = new List<IFilterToken>
                    {
                        new SearchEngineFilter.FilterToken
                        {
                            VariableName = expectedVariableTwoName,
                            VariableValue = expectedVariableTwoFirstValue,
                            VariableType = VariableTypeEnum.String,
                            FilterType = FilterTypeEnum.Equals
                        },
                        new SearchEngineFilter.FilterToken
                        {
                            VariableName = expectedVariableTwoName,
                            VariableValue = expectedVariableTwoSecondValue,
                            VariableType = VariableTypeEnum.String,
                            FilterType = FilterTypeEnum.Equals
                        }
                    },
                    Operation = FilterTokenGroupOperationEnum.Or
                }
            }
        };


        // Act
        var builder = new SearchEngineFilterBuilder();

        builder.WithFilterToken(
            expectedVariableOneName,
            expectedVariableOneFirstValue,
            VariableTypeEnum.String,
            FilterTypeEnum.Equals,
            out var variableOneFilterToken);
        builder.WithOrFilterToken(
            variableOneFilterToken,
            expectedVariableOneName,
            expectedVariableOneSecondValue,
            VariableTypeEnum.String,
            FilterTypeEnum.Equals,
            out _);

        builder.WithFilterToken(
            expectedVariableTwoName,
            expectedVariableTwoFirstValue,
            VariableTypeEnum.String,
            FilterTypeEnum.Equals,
            out var variableTwoFilterToken);
        builder.WithOrFilterToken(
            variableTwoFilterToken,
            expectedVariableTwoName,
            expectedVariableTwoSecondValue,
            VariableTypeEnum.String,
            FilterTypeEnum.Equals,
            out _);

        var filter = builder.Build();


        // Assert
        filter.WithDeepEqual(expectedFilter).IgnoreUnmatchedProperties().Assert();
    }
}
