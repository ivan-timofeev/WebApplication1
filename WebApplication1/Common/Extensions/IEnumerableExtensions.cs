namespace WebApplication1.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T>? source)
    {
        if (source is null || !source.Any())
            return true;
        return false;
    }
}
