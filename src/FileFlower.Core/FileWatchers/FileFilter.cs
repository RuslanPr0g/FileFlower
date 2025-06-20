using FileFlower.Core.Extensions;
using FileFlower.Core.FileWatchers.Contract;

namespace FileFlower.Core.FileWatchers;

/// <summary>
/// Represents a file filter that matches files against a specified pattern.
/// </summary>
/// <param name="pattern">The pattern to match file names against (e.g., "*.txt").</param>
public sealed class FileFilter(string pattern) : IFileFilter
{
    private readonly string _pattern = pattern;

    /// <summary>
    /// Determines whether the specified <paramref name="file"/> matches the filter pattern.
    /// </summary>
    /// <param name="file">The file information to evaluate.</param>
    /// <returns><c>true</c> if the file name matches the pattern; otherwise, <c>false</c>.</returns>
    public bool Matches(FileInfo file) => Path.GetFileName(file.FullName).Like(_pattern);
}
