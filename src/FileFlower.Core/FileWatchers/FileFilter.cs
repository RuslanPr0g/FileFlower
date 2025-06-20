using FileFlower.Core.Extensions;
using FileFlower.Core.FileWatchers.Contract;

namespace FileFlower.Core.FileWatchers;

public sealed class FileFilter(string pattern) : IFileFilter
{
    private readonly string _pattern = pattern;

    public bool Matches(FileInfo file) => Path.GetFileName(file.FullName).Like(_pattern);
}
