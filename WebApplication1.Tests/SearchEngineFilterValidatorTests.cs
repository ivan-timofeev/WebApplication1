using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Tests;

public class SearchEngineFilterValidatorTests
{
    [Theory]
    // Int
    [InlineData(FilterTypeEnum.MoreThan, AttributeTypeEnum.IntegerNumber, "1", nameof(DummyEntity.Field1))]
    [InlineData(FilterTypeEnum.LessThan, AttributeTypeEnum.IntegerNumber, "1", nameof(DummyEntity.Field1))]
    [InlineData(FilterTypeEnum.Equals, AttributeTypeEnum.IntegerNumber, "1", nameof(DummyEntity.Field1))]
    // Float
    [InlineData(FilterTypeEnum.MoreThan, AttributeTypeEnum.FloatNumber, "1.0", nameof(DummyEntity.Field2))]
    [InlineData(FilterTypeEnum.LessThan, AttributeTypeEnum.FloatNumber, "1.0", nameof(DummyEntity.Field2))]
    [InlineData(FilterTypeEnum.Equals, AttributeTypeEnum.FloatNumber, "1.0", nameof(DummyEntity.Field2))]
    // DateTime
    [InlineData(FilterTypeEnum.MoreThan, AttributeTypeEnum.DateTime, "01/01/2001 08:00", nameof(DummyEntity.Field3))]
    [InlineData(FilterTypeEnum.LessThan, AttributeTypeEnum.DateTime, "01/01/2001 08:00", nameof(DummyEntity.Field3))]
    [InlineData(FilterTypeEnum.Equals, AttributeTypeEnum.DateTime, "01/01/2001 08:00", nameof(DummyEntity.Field3))]
    // Guid
    [InlineData(FilterTypeEnum.Equals, AttributeTypeEnum.Guid, "7db0a735-7ef2-461b-aefa-2b56bc0ed109", nameof(DummyEntity.Field4))]
    // String
    [InlineData(FilterTypeEnum.Contains, AttributeTypeEnum.Text, "text", nameof(DummyEntity.Field5))]
    [InlineData(FilterTypeEnum.Equals, AttributeTypeEnum.Text, "text", nameof(DummyEntity.Field5))]
    [InlineData(FilterTypeEnum.StartWith, AttributeTypeEnum.Text, "text", nameof(DummyEntity.Field5))]
    // With access to sub entity "Field6.Field1"
    [InlineData(FilterTypeEnum.Equals, AttributeTypeEnum.IntegerNumber, "1", $"{nameof(DummyEntity.Field6)}.{nameof(DummySubEntity.Field1)}")]
    public void ValidateFilter_CorrectFilter_MustValidationPass(
        FilterTypeEnum filterType,
        AttributeTypeEnum attributeType,
        string attributeValue,
        string attributeName)
    {
        // Arrange
        var filter = new SearchEngineFilter()
        {
            FilterTokenGroups = new List<IFilterToken>
            {
                new SearchEngineFilter.FilterToken
                {
                    AttributeName = attributeName,
                    FilterType = filterType,
                    AttributeType = attributeType,
                    AttributeValue = attributeValue
                }
            }
        };


        // Act & Assert
        var validator = new SearchEngineFilterValidator();
        validator.ValidateFilter(filter, typeof(DummyEntity));
    }

    private record DummyEntity(int Field1, float Field2, DateTime Field3, Guid Field4, string Field5, DummySubEntity Field6);
    private record DummySubEntity(int Field1);
}
