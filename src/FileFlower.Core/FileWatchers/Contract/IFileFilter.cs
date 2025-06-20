namespace FileFlower.Core.FileWatchers.Contracts;

internal interface IFileFilter
{
    bool Matches(FileInfo file);
}