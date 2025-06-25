namespace FileFlower.Core.Abstractions;

/// <summary>
/// Defines a contract for filtering files based on custom criteria.
/// </summary>
public interface IFileFilter
{
    /// <summary>
    /// Determines whether the specified <paramref name="context"/> matches the filter criteria.
    /// </summary>
    /// <param name="context">The <see cref="FileContext"/> instance representing the file to evaluate.</param>
    /// <returns><c>true</c> if the file satisfies the filter condition; otherwise, <c>false</c>.</returns>
    bool Matches(FileContext context);
}