using WebApplication1.Abstraction.Models;

namespace WebApplication1.Implementation.Helpers.Extensions;

public static class ICollectionExtensions
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