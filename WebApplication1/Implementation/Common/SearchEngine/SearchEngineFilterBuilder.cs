namespace WebApplication1.Common.SearchEngine;

public class SearchEngineFilterBuilder
{
    private SearchEngineFilter _instance = new SearchEngineFilter();

    public SearchEngineFilterBuilder WithFilterToken(
        string variableName,
        string variableValue,
        AttributeTypeEnum attributeType,
        FilterTypeEnum filterType,
        out IFilterToken filterToken)
    {
        filterToken = new SearchEngineFilter.FilterToken
        {
            VariableName = variableName,
            AttributeValue = variableValue,
            AttributeType = attributeType,
            FilterType = filterType
        };

        _instance.FilterTokenGroups.Add(filterToken);
        return this;
    }

    public SearchEngineFilterBuilder WithOrFilterToken(
        IFilterToken destiny,
        string variableName,
        string variableValue,
        AttributeTypeEnum attributeType,
        FilterTypeEnum filterType,
        out IFilterToken filterToken)
    {
        var newFilterToken = new SearchEngineFilter.FilterToken
        {
            VariableName = variableName,
            AttributeValue = variableValue,
            AttributeType = attributeType,
            FilterType = filterType
        };

        var x = _instance.FilterTokenGroups.First(x => x == destiny);

        switch (x)
        {
            case SearchEngineFilter.FilterToken:
            {
                _instance.FilterTokenGroups.Remove(x);
            
                var group = new SearchEngineFilter.FilterTokenGroup()
                {
                    FilterTokens = new List<IFilterToken> { newFilterToken, destiny },
                    Operation = FilterTokenGroupOperationEnum.Or
                };
            
                _instance.FilterTokenGroups.Add(group);
                filterToken = group;
                return this;
            }
            case SearchEngineFilter.FilterTokenGroup filterTokenGroup:
            {
                filterTokenGroup.FilterTokens.Add(newFilterToken);
                filterToken = filterTokenGroup;
                return this;
            }
            default:
                throw new Exception();
        }
    }

    public SearchEngineFilterBuilder WithContains(
        string variableName,
        string variableValue,
        out IFilterToken filterToken)
    {
        WithFilterToken(
            variableName: variableName,
            variableValue: variableValue,
            attributeType: AttributeTypeEnum.String,
            filterType: FilterTypeEnum.Contains,
            out var createdToken);

        filterToken = createdToken;
        return this;
    }
    
    public SearchEngineFilterBuilder WithOrContains(
        IFilterToken destiny,
        string variableName,
        string variableValue,
        out IFilterToken filterToken)
    {
        WithOrFilterToken(
            destiny,
            variableName: variableName,
            variableValue: variableValue,
            attributeType: AttributeTypeEnum.String,
            filterType: FilterTypeEnum.Contains,
            out var createdToken);

        filterToken = createdToken;
        return this;
    }

    public SearchEngineFilter Build()
    {
        var created = _instance;
        _instance = new SearchEngineFilter();
        return created;
    }
}