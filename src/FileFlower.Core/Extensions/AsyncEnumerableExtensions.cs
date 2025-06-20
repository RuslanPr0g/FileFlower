/// <summary>
/// Provides asynchronous extension methods for <see cref="IEnumerable{T}"/> sequences,
/// enabling asynchronous filtering operations.
/// </summary>
public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// Filters an <see cref="IEnumerable{T}"/> asynchronously based on a predicate that returns a <see cref="Task{Boolean}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source sequence to filter.</param>
    /// <param name="predicate">An asynchronous function to test each element for a condition.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains an <see cref="IEnumerable{T}"/> of elements that satisfy the predicate.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="predicate"/> is <c>null</c>.</exception>
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
