using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Loggers;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.FileWatchers;

public class FileWatcherBuilder
{
    private readonly string _path;
    private readonly List<ProcessingRule> _rules = [];
    private readonly ILogger _logger;

    public FileWatcherBuilder(string path)
    {
        _path = path;
        _logger = ConsoleLogger.Create();
    }

    public FileWatcherBuilder(string path, ILogger logger)
    {
        Console.WriteLine($"Working with path: ${path}, while current path is {Environment.CurrentDirectory}");
        _path = path;
        _logger = logger;
    }

    public FileWatcherRuleBuilder WhenResourceCreated(Action<FileWatcherRuleBuilder> configure)
    {
        var ruleBuilder = new FileWatcherRuleBuilder(_logger);
        configure(ruleBuilder);
        _rules.Add(ruleBuilder.Build());
        return ruleBuilder;
    }

    public IFileWatcher Start()
    {
        var watcher = new FileWatcher(_path, _rules, _logger);
        watcher.Start();
        return watcher;
    }
}
