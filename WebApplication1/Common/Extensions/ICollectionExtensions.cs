namespace WebApplication1.Common.Extensions;

public static class CollectionExtensions
{
    public static void RemoveIfExists<TSource>(
        this ICollection<TSource> source,
        Func<TSource, bool> predicate)
        where TSource : class
    {
        var obj = source.FirstOrDefault(predicate);
        if (obj is null) return;

        source.Remove(obj);
    }
}
