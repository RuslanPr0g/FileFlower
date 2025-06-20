namespace FileFlower.Core.FileWatchers;

public class FileWatcherBuilder(string path)
{
    private readonly string _path = path;
    private readonly List<IFileFilter> _filters = new();
    private readonly List<IFileProcessingStep> _steps = new();

    public FileWatcherBuilder Filter(string pattern)
    {
        _filters.Add(new FileFilter(pattern));
        return this;
    }

    public FileWatcherBuilder AddStep(Func<FileInfo, Task> handler)
    {
        _steps.Add(new DelegateProcessingStep(handler));
        return this;
    }

    public IFileWatcher Start()
    {
        var pipeline = new FileProcessingPipeline(_steps);
        var watcher = new FileWatcher(_path, _filters, pipeline);
        watcher.Start();
        return watcher;
    }
}
