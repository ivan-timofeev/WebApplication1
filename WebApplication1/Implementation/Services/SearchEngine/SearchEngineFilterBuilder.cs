using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine;

public class SearchEngineFilterBuilder
{
    private SearchEngineFilter _instance = new SearchEngineFilter();

    public SearchEngineFilterBuilder WithFilterToken(
        string attributeName,
        string attributeValue,
        AttributeTypeEnum attributeType,
        FilterTypeEnum filterType,
        out IFilterToken filterToken)
    {
        filterToken = new SearchEngineFilter.FilterToken
        {
            AttributeName = attributeName,
            AttributeValue = attributeValue,
            AttributeType = attributeType,
            FilterType = filterType
        };

        _instance.FilterTokenGroups.Add(filterToken);
        return this;
    }

    public SearchEngineFilterBuilder WithOrFilterToken(
        IFilterToken destiny,
        string attributeName,
        string attributeValue,
        AttributeTypeEnum attributeType,
        FilterTypeEnum filterType,
        out IFilterToken filterToken)
    {
        var newFilterToken = new SearchEngineFilter.FilterToken
        {
            AttributeName = attributeName,
            AttributeValue = attributeValue,
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
                    FilterTokens = new List<IFilterToken> { destiny, newFilterToken },
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
    
    public SearchEngineFilterBuilder WithEquals(
        string attributeName,
        string attributeValue,
        AttributeTypeEnum attributeType,
        out IFilterToken filterToken)
    {
        WithFilterToken(
            attributeName: attributeName,
            attributeValue: attributeValue,
            attributeType: attributeType,
            filterType: FilterTypeEnum.Equals,
            out var createdToken);

        filterToken = createdToken;
        return this;
    }

    public SearchEngineFilterBuilder WithContains(
        string attributeName,
        string attributeValue,
        out IFilterToken filterToken)
    {
        WithFilterToken(
            attributeName: attributeName,
            attributeValue: attributeValue,
            attributeType: AttributeTypeEnum.Text,
            filterType: FilterTypeEnum.Contains,
            out var createdToken);

        filterToken = createdToken;
        return this;
    }
    
    public SearchEngineFilterBuilder WithOrContains(
        IFilterToken destiny,
        string attributeName,
        string attributeValue,
        out IFilterToken filterToken)
    {
        WithOrFilterToken(
            destiny,
            attributeName: attributeName,
            attributeValue: attributeValue,
            attributeType: AttributeTypeEnum.Text,
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
