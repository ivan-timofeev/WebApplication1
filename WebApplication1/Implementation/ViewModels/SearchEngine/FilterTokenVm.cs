#nullable disable
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.ViewModels.SearchEngine;

public class FilterTokenVm : FilterTokenBaseVm
{
    public string AttributeName { get; init; }
    public string AttributeValue { get; init; }
    public AttributeTypeEnum AttributeType { get; init; }
    public FilterTypeEnum FilterType { get; init; }
}
