namespace FileFlower.Core.FileWatchers;

internal sealed class FileFilter(string pattern) : IFileFilter
{
    private readonly string _pattern = pattern;

    public bool Matches(FileInfo file) => System.IO.Path.GetFileName(file.FullName).Like(_pattern);
}
