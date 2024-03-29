#nullable disable
using WebApplication1.Abstractions.Services.SearchEngine;
namespace WebApplication1.Services.SearchEngine.Models;

public class SearchEngineFilter
{
    public List<IFilterToken> FilterTokenGroups { get; init; } = new List<IFilterToken>();

    public class FilterTokenGroup : IFilterToken
    {
        public List<IFilterToken> FilterTokens { get; init; }
        public FilterTokenGroupOperationEnum Operation { get; init; }
    }

    public class FilterToken : IFilterToken
    {
        public string AttributeName { get; init; }
        public string AttributeValue { get; init; }
        public AttributeTypeEnum AttributeType { get; init; }
        public FilterTypeEnum FilterType { get; init; }
    }
}
