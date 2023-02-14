using System.Text.RegularExpressions;
using AutoMapper.Internal;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Common.SearchEngine;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Services.SearchEngine;

public class SearchEngineFilterValidator : ISearchEngineFilterValidator
{
    public void ValidateFilter(SearchEngineFilter filter, Type entityType)
    {
        var filterTokens = CollectFilterTokens(filter);
        BaseValidation(filterTokens);
        TypeValidation(filterTokens, entityType);
    }

    private void BaseValidation(SearchEngineFilter.FilterToken[] filterTokens)
    {
        foreach (var item in filterTokens)
        {
            ValidateFilterToken(item);
        }
    }

    private void TypeValidation(SearchEngineFilter.FilterToken[] filterTokens, Type entityType)
    {
        foreach (var item in filterTokens)
        {
            ValidateFilterTokenByType(item, entityType);
        }
    }

    private static void ValidateFilterToken(SearchEngineFilter.FilterToken filterToken)
    {
        var filterType = filterToken.FilterType;
        var attributeType = filterToken.AttributeType;
        
        if (filterType is FilterTypeEnum.LessThan or FilterTypeEnum.GreaterThan)
        {
            if (attributeType is not (AttributeTypeEnum.FloatNumber or AttributeTypeEnum.IntegerNumber or AttributeTypeEnum.DateTime))
            {
                throw new SearchEngineFilterValidationException(message:
                    string.Format("Filter type \"{0}\" cannot be used with value type \"{1}\".",
                        filterType,
                        attributeType));
            }
        }

        if (filterType is FilterTypeEnum.Contains or FilterTypeEnum.StartsWith)
        {
            if (attributeType is not (AttributeTypeEnum.Text))
            {
                throw new SearchEngineFilterValidationException(message:
                    string.Format("Filter type \"{0}\" cannot be used with value type \"{1}\".",
                        filterType,
                        attributeType));
            }
        }
    }
    
    private static void ValidateFilterTokenByType(SearchEngineFilter.FilterToken filterToken, Type entityType)
    {
        var attributeType = GetAttributeType(filterToken.AttributeName, entityType);

        if (attributeType is null)
        {
            throw new SearchEngineFilterValidationException(message:
                string.Format("Entity \"{0}\" does not contain attribute with name \"{1}\".",
                    entityType.Name,
                    filterToken.AttributeName));
        }

        var attributeTypeName = GetAttributeTypeName(attributeType);
        var filterAttributeTypeName = filterToken.AttributeType.ToString().ToLower();

        if (!filterAttributeTypeName.Contains(attributeTypeName))
        {
            throw new SearchEngineFilterValidationException(message:
                string.Format("Provided attribute type \"{0}\" does not match actual attribute type \"{1}\".",
                    filterAttributeTypeName,
                    attributeTypeName));
        }
    }

    private static Type? GetAttributeType(string attributeName, Type entityType)
    {
        var split = attributeName.ToLower().Split(".");
        var buffer = entityType;

        foreach (var attributePathPart in split)
        {
            buffer = buffer.GetProperties()
                .First(x => attributePathPart.ToLower().Contains(x.Name.ToLower()))
                .PropertyType;
        }

        return buffer;
    }

    private static string GetAttributeTypeName(Type attributeType)
    {
        var attributeTypeName = attributeType.IsNullableType()
            ? attributeType.GetGenericArguments()[0].Name
            : attributeType.Name;
        
        var typeName = attributeTypeName
            .ToLower()
            .Replace("single", "float")
            .Replace("double", "float")
            .Replace("string", "text");
        return Regex.Replace(typeName, @"[\d-]", string.Empty);
    }

    private static SearchEngineFilter.FilterToken[] CollectFilterTokens(SearchEngineFilter filter)
    {
        var tokens = new List<SearchEngineFilter.FilterToken>();

        foreach (var item in filter.FilterTokenGroups)
        {
            switch (item)
            {
                case SearchEngineFilter.FilterTokenGroup filterTokenGroup:
                    tokens.AddRange(CollectFilterTokens(filterTokenGroup));
                    break;
                case SearchEngineFilter.FilterToken filterToken:
                    tokens.Add(filterToken);
                    break;
            }
        }

        return tokens.ToArray();
    }

    private static List<SearchEngineFilter.FilterToken> CollectFilterTokens(SearchEngineFilter.FilterTokenGroup input)
    {
        var tokens = new List<SearchEngineFilter.FilterToken>();
        
        foreach (var item in input.FilterTokens)
        {
            switch (item)
            {
                case SearchEngineFilter.FilterTokenGroup filterTokenGroup:
                    tokens.AddRange(CollectFilterTokens(filterTokenGroup));
                    break;
                case SearchEngineFilter.FilterToken filterToken:
                    tokens.Add(filterToken);
                    break;
            }
        }

        return tokens;
    }
}
