namespace FileFlower.Core.FileWatchers.Contract;

public interface IFileWatcher : IDisposable
{
    void Start();
}
