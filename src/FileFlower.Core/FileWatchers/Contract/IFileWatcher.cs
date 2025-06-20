namespace FileFlower.Core.FileWatchers.Contracts;

public interface IFileWatcher : IDisposable
{
    void Start();
}
