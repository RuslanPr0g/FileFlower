using FileFlower.Core.Abstractions;
using FileFlower.Core.Rules;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.FileWatchers;

/// <summary>
/// Watches a directory (and optionally its subdirectories) for file system changes,
/// and dispatches file events to configured processing rules.
/// </summary>
public sealed class FileWatcher : IFileWatcher, IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly List<ProcessingRule> _rules;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileWatcher"/> class that monitors the specified <paramref name="path"/>
    /// and applies the given processing <paramref name="rules"/>.
    /// </summary>
    /// <param name="path">The directory path to watch.</param>
    /// <param name="rules">A collection of processing rules to apply on file events.</param>
    /// <param name="logger">An <see cref="ILogger"/> instance for logging.</param>
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

    /// <summary>
    /// Starts the file watcher, enabling it to monitor file changes and raise events accordingly.
    /// </summary>
    public void Start()
    {
        _logger.LogInformation("Starting FileWatcher for path: {Path}", _watcher.Path);
        _watcher.IncludeSubdirectories = true;
        _watcher.EnableRaisingEvents = true;
    }

    // TODO: is async void bad here?

    private async void OnCreated(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Created)
            return;

        var context = GetFileContext(e, FileModificationType.Created);
        await HandleUpdates(e, (rule) => rule.TryProcessAsync(context), context.FileModificationType);
    }

    private async void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
            return;

        var context = GetFileContext(e, FileModificationType.Changed);
        await HandleUpdates(e, (rule) => rule.TryProcessAsync(context), context.FileModificationType);
    }

    private async void OnDeleted(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Deleted)
            return;

        var context = GetFileContext(e, FileModificationType.Deleted);
        await HandleUpdates(e, (rule) => rule.TryProcessAsync(context), context.FileModificationType);
    }

    private async void OnRenamed(object sender, RenamedEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Renamed)
            return;

        var context = GetFileContext(e, FileModificationType.Renamed);
        await HandleUpdates(e, (rule) => rule.TryProcessAsync(context), context.FileModificationType);
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        var ex = e.GetException();
        if (ex != null)
            _logger.LogError(ex, "File system watcher error: {Message}", ex.Message);
    }

    private async Task HandleUpdates(
        FileSystemEventArgs e,
        Func<ProcessingRule, Task<bool>> predicate,
        FileModificationType operation = FileModificationType.NotSpecified)
    {
        var processed = await _rules.WhereAsync(predicate);

        if (processed.Any())
        {
            _logger.LogInformation(
                "File {UpdateType}: {FilePath}, under {Directory}",
                operation.ToString(),
                e.FullPath,
                _watcher.Path);
        }
        else
        {
            _logger.LogDebug(
                "File {UpdateType} but did not match filters: {FilePath}, under {Directory}",
                operation.ToString(),
                e.FullPath,
                _watcher.Path);
        }
    }

    private static FileContext GetFileContext(FileSystemEventArgs e, FileModificationType operation)
    {
        return new(new(e.FullPath), operation);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="FileWatcher"/> instance.
    /// </summary>
    public void Dispose()
    {
        _watcher.EnableRaisingEvents = false;
        _logger.LogInformation("Disposing FileWatcher for path: {Path}", _watcher.Path);
        _watcher.Dispose();
    }
}
