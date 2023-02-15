#nullable disable
using System.ComponentModel.DataAnnotations;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.ViewModels.SearchEngine;

public class FilterTokenVm : FilterTokenBaseVm
{
    [Required]
    public string AttributeName { get; init; }
    [Required]
    public string AttributeValue { get; init; }
    [Required]
    public AttributeTypeEnum AttributeType { get; init; }
    [Required]
    public FilterTypeEnum FilterType { get; init; }
}
