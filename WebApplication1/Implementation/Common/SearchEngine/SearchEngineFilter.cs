#nullable disable
namespace WebApplication1.Common.SearchEngine;

public interface IFilterToken
{
        
}

public enum FilterTokenGroupOperationEnum
{
    And,
    Or
}

public enum AttributeTypeEnum
{
    String,
    Int,
    Float,
    DateTime,
    Guid
}
    
public enum FilterTypeEnum
{
    Equals,
    Contains,
    MoreThan,
    LessThan,
    StartWith
}

public class SearchEngineFilter
{
    public List<IFilterToken> FilterTokenGroups { get; init; } = new List<IFilterToken>();
    public FilterTokenGroupOperationEnum Operation => FilterTokenGroupOperationEnum.And;

    public class FilterTokenGroup : IFilterToken
    {
        public List<IFilterToken> FilterTokens { get; set; }
        public FilterTokenGroupOperationEnum Operation { get; set; }
    }
    
    public class FilterToken : IFilterToken
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public AttributeTypeEnum AttributeType { get; set; }
        public FilterTypeEnum FilterType { get; set; }
    }
}
