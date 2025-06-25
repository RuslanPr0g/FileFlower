using FileFlower.Core.Abstractions;
using FileFlower.Core.Loggers;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.FileWatchers;

/// <summary>
/// Provides a fluent builder compositor for configuring multiple file watcher builders to watch multiple directories
/// associated with specific file system events.
/// </summary>
public class FileWatcherBuilderCompositor
{
    private readonly ILogger _logger;
    private readonly Dictionary<string, FileWatcherBuilder> _builders = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FileWatcherBuilderCompositor"/> class
    /// </summary>
    public FileWatcherBuilderCompositor()
    {
        _logger = ConsoleLogger.Create();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileWatcherBuilderCompositor"/> class
    /// a custom <paramref name="logger"/> for diagnostic output.
    /// </summary>
    /// <param name="logger">The logger to use for diagnostic messages.</param>
    public FileWatcherBuilderCompositor(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <summary>
    /// Configures a file watcher builder for a specified directory.
    /// </summary>
    /// <param name="path">The directory path to watch.</param>
    /// <param name="configure">Configuration of the file watcher builder.</param>
    /// <returns>File watcher builder instance.</returns>
    /// <exception cref="ArgumentException">When the provided path has been added to the configuration previously.</exception>
    public FileWatcherBuilder ForDirectory(
        string path,
        Action<FileWatcherBuilder> configure)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(configure);

        if (_builders.ContainsKey(path))
        {
            throw new ArgumentException($"The path has been added to the configuration already. Path: {path}.");
        }

        var builder = new FileWatcherBuilder(path, _logger);
        configure(builder);

        _builders.Add(path, builder);

        return builder;
    }

    /// <summary>
    /// Composes and builds all the configured file watcher builders.
    /// </summary>
    /// <returns>The list of built file watchers.</returns>
    public IEnumerable<IFileWatcher> Compose()
    {
        return _builders.Values.Select(_ => _.Build());
    }
}
