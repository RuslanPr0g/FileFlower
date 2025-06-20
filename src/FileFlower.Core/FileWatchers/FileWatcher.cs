using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Pipelines;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.FileWatchers;

public sealed class FileWatcher : IFileWatcher, IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly IEnumerable<IFileFilter> _filters;
    private readonly FileProcessingPipeline _pipeline;
    private readonly ILogger<FileWatcher> _logger;

    public FileWatcher(
        string path,
        IEnumerable<IFileFilter> filters,
        FileProcessingPipeline pipeline,
        ILogger<FileWatcher> logger)
    {
        _filters = filters;
        _pipeline = pipeline;
        _logger = logger;

        _watcher = new FileSystemWatcher(path)
        {
            IncludeSubdirectories = true,
            // TODO: should be configurable by client
            NotifyFilter = NotifyFilters.Attributes
                         | NotifyFilters.CreationTime
                         | NotifyFilters.DirectoryName
                         | NotifyFilters.FileName
                         | NotifyFilters.LastAccess
                         | NotifyFilters.LastWrite
                         | NotifyFilters.Security
                         | NotifyFilters.Size
        };

        _watcher.Created += OnCreated;
        _watcher.Changed += OnChanged;
        _watcher.Deleted += OnDeleted;
        _watcher.Renamed += OnRenamed;
        _watcher.Error += OnError;
    }

    public void Start()
    {
        _logger.LogInformation("Starting FileWatcher for path: {Path}", _watcher.Path);
        _watcher.IncludeSubdirectories = true;
        _watcher.EnableRaisingEvents = true;
    }

    private async void OnCreated(object sender, FileSystemEventArgs e)
    {
        var file = new FileInfo(e.FullPath);
        if (_filters.All(f => f.Matches(file)))
        {
            _logger.LogInformation("File created: {FilePath}", e.FullPath);
            await _pipeline.ExecuteAsync(file);
        }
        else
        {
            _logger.LogDebug("File created but did not match filters: {FilePath}", e.FullPath);
        }
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
            return;

        _logger.LogInformation("File changed: {FilePath}", e.FullPath);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("File deleted: {FilePath}", e.FullPath);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        _logger.LogInformation("File renamed from {OldPath} to {NewPath}", e.OldFullPath, e.FullPath);
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        var ex = e.GetException();
        if (ex != null)
            _logger.LogError(ex, "File system watcher error: {Message}", ex.Message);
    }

    public void Dispose()
    {
        _logger.LogInformation("Disposing FileWatcher for path: {Path}", _watcher.Path);
        _watcher.Dispose();
    }
}
