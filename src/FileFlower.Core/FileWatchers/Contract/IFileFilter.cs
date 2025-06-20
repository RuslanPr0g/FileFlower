namespace FileFlower.Core.FileWatchers.Contract;

public interface IFileFilter
{
    bool Matches(FileInfo file);
}