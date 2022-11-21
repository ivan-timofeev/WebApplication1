using AutoMapper;
using WebApplication1.Abstraction.Models;

namespace WebApplication1.Implementation.Helpers.Extensions;

public static class PagedModelExtensions
{
    private static readonly Mapper Mapper = new Mapper(AutomapperConfiguration.GetAutomapperConfiguration());
    
    public static PagedModel<TDestination> MapTo<TDestination>(this PagedModel x)
    {
        return new PagedModel<TDestination>(
            x.Items.Cast<object>().Select(c => Mapper.Map<TDestination>(c)),
            x.Page,
            x.PageSize,
            x.TotalItemsCount);
    }
}
