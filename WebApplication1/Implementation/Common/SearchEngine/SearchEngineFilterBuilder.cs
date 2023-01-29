namespace WebApplication1.Common.SearchEngine;

public class SearchEngineFilterBuilder
{
    private readonly SearchEngineFilter _instance = new SearchEngineFilter();

    public SearchEngineFilterBuilder WithFilterToken(
        string variableName,
        string variableValue,
        VariableTypeEnum variableType,
        FilterTypeEnum filterType,
        out IFilterToken filterToken)
    {
        filterToken = new SearchEngineFilter.FilterToken
        {
            VariableName = variableName,
            VariableValue = variableValue,
            VariableType = variableType,
            FilterType = filterType
        };

        _instance.FilterTokenGroups.Add(filterToken);
        return this;
    }

    public SearchEngineFilterBuilder WithOrFilterToken(
        IFilterToken destiny,
        string variableName,
        string variableValue,
        VariableTypeEnum variableType,
        FilterTypeEnum filterType,
        out IFilterToken filterToken)
    {
        var newFilterToken = new SearchEngineFilter.FilterToken
        {
            VariableName = variableName,
            VariableValue = variableValue,
            VariableType = variableType,
            FilterType = filterType
        };

        var x = _instance.FilterTokenGroups.First(x => x == destiny);
        _instance.FilterTokenGroups.Remove(x);

        var group = new SearchEngineFilter.FilterTokenGroup()
        {
            FilterTokens = new IFilterToken[] { newFilterToken, destiny },
            Operation = FilterTokenGroupOperationEnum.Or
        };
        _instance.FilterTokenGroups.Add(group);
        filterToken = group;
        return this;
    }

    public SearchEngineFilter Build()
    {
        return _instance;
    }
}