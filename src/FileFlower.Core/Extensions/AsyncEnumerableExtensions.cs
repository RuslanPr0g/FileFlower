public static class AsyncEnumerableExtensions
{
    public static async Task<IEnumerable<T>> WhereAsync<T>(
        this IEnumerable<T> source,
        Func<T, Task<bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        var results = new List<T>();

        foreach (var item in source)
        {
            if (await predicate(item))
            {
                results.Add(item);
            }
        }

        return results;
    }
}
