using FileFlower.Core.Abstractions;
using FileFlower.Core.Extensions;

namespace FileFlower.Core.Filters;

/// <summary>
/// Represents a file filter that matches file's names against a specified pattern.
/// </summary>
/// <param name="pattern">The pattern to match file names against (e.g., "*.txt").</param>
public sealed class FileNameFilter(string pattern) : IFileFilter
{
    private readonly string _pattern = pattern;

    /// <summary>
    /// Determines whether the specified <paramref name="context"/> matches the filter pattern.
    /// </summary>
    /// <param name="context">The file information to evaluate.</param>
    /// <returns><c>true</c> if the file name matches the pattern; otherwise, <c>false</c>.</returns>
    public bool Matches(FileContext context) => Path.GetFileName(context.FileInfo.FullName).Like(_pattern);
}
