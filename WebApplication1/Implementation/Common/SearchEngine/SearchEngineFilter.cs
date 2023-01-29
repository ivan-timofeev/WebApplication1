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

public enum VariableTypeEnum
{
    String,
    IntegerNumber,
    FloatNumber,
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
        public IFilterToken[] FilterTokens { get; set; }
        public FilterTokenGroupOperationEnum Operation { get; set; }
    }
    
    public class FilterToken : IFilterToken
    {
        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        public VariableTypeEnum VariableType { get; set; }
        public FilterTypeEnum FilterType { get; set; }
    }
}
