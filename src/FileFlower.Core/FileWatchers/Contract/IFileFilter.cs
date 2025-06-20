namespace FileFlower.Core.FileWatchers.Contract;

/// <summary>
/// Defines a contract for filtering files based on custom criteria.
/// </summary>
public interface IFileFilter
{
    /// <summary>
    /// Determines whether the specified <paramref name="file"/> matches the filter criteria.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> instance representing the file to evaluate.</param>
    /// <returns><c>true</c> if the file satisfies the filter condition; otherwise, <c>false</c>.</returns>
    bool Matches(FileInfo file);
}