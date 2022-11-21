using System.Collections;

namespace WebApplication1.Abstraction.Models;

public class PagedModel
{
    public IEnumerable Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int MaxPage { get; }
    public int TotalItemsCount { get; }

    public PagedModel(
        IEnumerable? items,
        int page,
        int pageSize,
        int totalItemsCount)
    {
        Items = items ?? Array.Empty<object>();
        Page = page >= 1 ? page : 1;
        PageSize = pageSize >= 1 ? pageSize : 1;
        TotalItemsCount = totalItemsCount;
        MaxPage = (int) Math.Ceiling(totalItemsCount / (double)pageSize);
    }
}

public class PagedModel<T> : PagedModel
{
    public new IEnumerable<T> Items { get; }

    public PagedModel(
        IEnumerable<T>? items,
        int page,
        int pageSize,
        int totalItemsCount)
        : base(items, page, pageSize, totalItemsCount)
    {
        Items = items ?? Array.Empty<T>();
    }
}
