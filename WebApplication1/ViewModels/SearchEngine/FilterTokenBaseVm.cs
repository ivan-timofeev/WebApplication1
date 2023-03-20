#nullable disable
using System.Text.Json.Serialization;

namespace WebApplication1.ViewModels.SearchEngine;

[JsonDerivedType(typeof(FilterTokenGroupVm), "group")]
[JsonDerivedType(typeof(FilterTokenVm), "token")]
public class FilterTokenBaseVm
{
    
}
