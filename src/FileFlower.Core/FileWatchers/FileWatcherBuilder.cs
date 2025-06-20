using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Loggers;
using FileFlower.Core.Pipelines;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.FileWatchers;

public class FileWatcherBuilder
{
    private readonly string _path;
    private readonly List<IFileFilter> _filters = [];
    private readonly List<IFileProcessingStep> _steps = [];
    private readonly ILogger<FileWatcher> _logger;

    public FileWatcherBuilder(string path)
    {
        _path = path;
        _logger = ConsoleLogger<FileWatcher>.Create();
    }

    public FileWatcherBuilder(string path, ILogger<FileWatcher> logger)
    {
        _path = path;
        _logger = logger;
    }

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
        var watcher = new FileWatcher(_path, _filters, pipeline, _logger);
        watcher.Start();
        return watcher;
    }
}
