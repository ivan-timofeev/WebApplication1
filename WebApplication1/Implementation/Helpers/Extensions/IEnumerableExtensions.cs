using WebApplication1.Abstraction.Models;

namespace WebApplication1.Implementation.Helpers.Extensions;

public static class IEnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T>? source)
    {
        if (source is null || !source.Any())
            return true;
        return false;
    }
}