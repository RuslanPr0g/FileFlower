using FileFlower.Core.FileWatchers.Contract;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.FileWatchers;

public sealed class FileWatcher : IFileWatcher, IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly List<ProcessingRule> _rules;
    private readonly ILogger _logger;

    public FileWatcher(
        string path,
        List<ProcessingRule> rules,
        ILogger logger)
    {
        _rules = rules;
        _logger = logger;

        if (!Directory.Exists(path))
        {
            _logger.LogWarning("Directory '{Path}' does not exist. Creating it.", path);
            Directory.CreateDirectory(path);
        }

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

        var processed = await _rules.WhereAsync(_ => _.TryProcessAsync(file));

        if (processed.Any())
        {
            _logger.LogInformation("File created: {FilePath}", e.FullPath);
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
        _watcher.EnableRaisingEvents = false;
        _logger.LogInformation("Disposing FileWatcher for path: {Path}", _watcher.Path);
        _watcher.Dispose();
    }
}
