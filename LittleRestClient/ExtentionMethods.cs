namespace LittleRestClient;

internal static class ExtentionMethods
{
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
    {
        return source ?? Enumerable.Empty<T>();
    }
    public static IDictionary<T,S> EmptyIfNull<T,S>(this IDictionary<T,S>? source)
    {
        return source ?? new Dictionary<T,S>();
    }
}
