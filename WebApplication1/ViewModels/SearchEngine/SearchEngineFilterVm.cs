#nullable disable
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.SearchEngine;

public class SearchEngineFilterVm
{
    [Required, MinLength(1)]
    public IEnumerable<FilterTokenBaseVm> FilterTokenGroups { get; init; }
}
