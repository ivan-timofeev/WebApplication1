#nullable disable
using System.ComponentModel.DataAnnotations;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.ViewModels.SearchEngine;

public class FilterTokenGroupVm : FilterTokenBaseVm
{
    [Required, MinLength(1)]
    public IEnumerable<FilterTokenBaseVm> FilterTokens { get; init; }
    [Required]
    public FilterTokenGroupOperationEnum Operation { get; init; }
}
