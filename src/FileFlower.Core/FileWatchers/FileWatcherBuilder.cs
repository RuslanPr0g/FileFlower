using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Loggers;
using FileFlower.Core.Rules;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.FileWatchers;

/// <summary>
/// Provides a fluent builder for configuring and registering file processing rules
/// associated with specific file system events.
/// </summary>
public class FileWatcherBuilder
{
    private readonly string _path;
    private readonly List<ProcessingRule> _rules = [];
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileWatcherBuilder"/> class
    /// with the specified directory <paramref name="path"/> to watch.
    /// </summary>
    /// <param name="path">The directory path to monitor for file system changes.</param>
    public FileWatcherBuilder(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _path = path;
        _logger = ConsoleLogger.Create();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileWatcherBuilder"/> class
    /// with the specified directory <paramref name="path"/> to watch and
    /// a custom <paramref name="logger"/> for diagnostic output.
    /// </summary>
    /// <param name="path">The directory path to monitor for file system changes.</param>
    /// <param name="logger">The logger to use for diagnostic messages.</param>
    public FileWatcherBuilder(string path, ILogger logger)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _path = path;
        _logger = logger;
    }

    /// <summary>
    /// Defines a processing rule triggered when a resource is created in the watched directory.
    /// </summary>
    /// <param name="configure">An action to configure the <see cref="FileWatcherRuleBuilder"/> for this rule.</param>
    /// <returns>The configured <see cref="FileWatcherRuleBuilder"/> instance for further customization.</returns>
    public FileWatcherRuleBuilder WhenResourceCreated(Action<FileWatcherRuleBuilder> configure)
    {
        var ruleBuilder = new FileWatcherRuleBuilder(_logger);
        configure(ruleBuilder);
        _rules.Add(ruleBuilder.BuildWithOperationCreated());
        return ruleBuilder;
    }

    /// <summary>
    /// Defines a processing rule triggered when a resource is changed in the watched directory.
    /// </summary>
    /// <param name="configure">An action to configure the <see cref="FileWatcherRuleBuilder"/> for this rule.</param>
    /// <returns>The configured <see cref="FileWatcherRuleBuilder"/> instance for further customization.</returns>
    public FileWatcherRuleBuilder WhenResourceChanged(Action<FileWatcherRuleBuilder> configure)
    {
        var ruleBuilder = new FileWatcherRuleBuilder(_logger);
        configure(ruleBuilder);
        _rules.Add(ruleBuilder.BuildWithOperationChanged());
        return ruleBuilder;
    }

    /// <summary>
    /// Defines a processing rule triggered when a resource is renamed in the watched directory.
    /// </summary>
    /// <param name="configure">An action to configure the <see cref="FileWatcherRuleBuilder"/> for this rule.</param>
    /// <returns>The configured <see cref="FileWatcherRuleBuilder"/> instance for further customization.</returns>
    public FileWatcherRuleBuilder WhenResourceRenamed(Action<FileWatcherRuleBuilder> configure)
    {
        var ruleBuilder = new FileWatcherRuleBuilder(_logger);
        configure(ruleBuilder);
        _rules.Add(ruleBuilder.BuildWithOperationRenamed());
        return ruleBuilder;
    }

    /// <summary>
    /// Defines a processing rule triggered when a resource is deleted from the watched directory.
    /// </summary>
    /// <param name="configure">An action to configure the <see cref="FileWatcherRuleBuilder"/> for this rule.</param>
    /// <returns>The configured <see cref="FileWatcherRuleBuilder"/> instance for further customization.</returns>
    public FileWatcherRuleBuilder WhenResourceDeleted(Action<FileWatcherRuleBuilder> configure)
    {
        var ruleBuilder = new FileWatcherRuleBuilder(_logger);
        configure(ruleBuilder);
        _rules.Add(ruleBuilder.BuildWithOperationDeleted());
        return ruleBuilder;
    }

    /// <summary>
    /// Builds and starts the configured <see cref="IFileWatcher"/> instance to begin monitoring.
    /// </summary>
    /// <returns>An <see cref="IFileWatcher"/> instance that is actively monitoring the specified directory.</returns>
    public IFileWatcher Build()
    {
        var watcher = new FileWatcher(_path, _rules, _logger);
        watcher.Start();
        return watcher;
    }
}
