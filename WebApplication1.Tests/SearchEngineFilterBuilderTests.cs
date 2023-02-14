using DeepEqual.Syntax;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.Models;

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
                            AttributeName = expectedVariableOneName,
                            AttributeValue = expectedVariableOneFirstValue,
                            AttributeType = AttributeTypeEnum.Text,
                            FilterType = FilterTypeEnum.Equals
                        },
                        new SearchEngineFilter.FilterToken
                        {
                            AttributeName = expectedVariableOneName,
                            AttributeValue = expectedVariableOneSecondValue,
                            AttributeType = AttributeTypeEnum.Text,
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
                            AttributeName = expectedVariableTwoName,
                            AttributeValue = expectedVariableTwoFirstValue,
                            AttributeType = AttributeTypeEnum.Text,
                            FilterType = FilterTypeEnum.Equals
                        },
                        new SearchEngineFilter.FilterToken
                        {
                            AttributeName = expectedVariableTwoName,
                            AttributeValue = expectedVariableTwoSecondValue,
                            AttributeType = AttributeTypeEnum.Text,
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
            AttributeTypeEnum.Text,
            FilterTypeEnum.Equals,
            out var variableOneFilterToken);
        builder.WithOrFilterToken(
            variableOneFilterToken,
            expectedVariableOneName,
            expectedVariableOneSecondValue,
            AttributeTypeEnum.Text,
            FilterTypeEnum.Equals,
            out _);

        builder.WithFilterToken(
            expectedVariableTwoName,
            expectedVariableTwoFirstValue,
            AttributeTypeEnum.Text,
            FilterTypeEnum.Equals,
            out var variableTwoFilterToken);
        builder.WithOrFilterToken(
            variableTwoFilterToken,
            expectedVariableTwoName,
            expectedVariableTwoSecondValue,
            AttributeTypeEnum.Text,
            FilterTypeEnum.Equals,
            out _);

        var filter = builder.Build();


        // Assert
        filter.WithDeepEqual(expectedFilter)
            .IgnoreUnmatchedProperties()
            .Assert();
    }
}
