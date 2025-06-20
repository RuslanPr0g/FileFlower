using FileFlower.Core.FileWatchers;
using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Loggers;
using FileFlower.Core.Pipelines;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core;

public sealed class FileWatcherRuleBuilder
{
    private readonly List<IFileFilter> _filters = [];
    private readonly FileProcessingPipeline _pipeline = new();
    private ProcessingRuleCondition _useAndLogic = ProcessingRuleCondition.And;
    private ProcessingRuleOperation _operation = ProcessingRuleOperation.NotSpecified;

    private readonly ILogger _logger;

    public FileWatcherRuleBuilder(ILogger logger)
    {
        _logger = logger;
    }

    public FileWatcherRuleBuilder()
    {
        _logger = ConsoleLogger.Create();
    }

    public FileWatcherRuleBuilder Filter(string pattern)
    {
        var filter = new FileFilter(pattern);
        _filters.Add(filter);
        return this;
    }

    /// <summary>
    /// Switch filter logic to OR
    /// </summary>
    public FileWatcherRuleBuilder UseOrLogic()
    {
        _useAndLogic = ProcessingRuleCondition.Or;
        return this;
    }

    /// <summary>
    /// Switch filter logic to AND (default)
    /// </summary>
    public FileWatcherRuleBuilder UseAndLogic()
    {
        _useAndLogic = ProcessingRuleCondition.And;
        return this;
    }

    public FileWatcherRuleBuilder AddStep(Func<FileInfo, Task> step)
    {
        _pipeline.AddStep(step);
        return this;
    }

    internal ProcessingRule BuildWithOperationCreated()
    {
        return Build(ProcessingRuleOperation.Created);
    }

    internal ProcessingRule BuildWithOperationChanged()
    {
        return Build(ProcessingRuleOperation.Changed);
    }

    internal ProcessingRule BuildWithOperationDeleted()
    {
        return Build(ProcessingRuleOperation.Deleted);
    }

    internal ProcessingRule BuildWithOperationRenamed()
    {
        return Build(ProcessingRuleOperation.Renamed);
    }

    internal ProcessingRule Build(ProcessingRuleOperation operation)
    {
        _operation = operation;
        return Build();
    }

    internal ProcessingRule Build()
    {
        if (_filters.Count == 0)
        {
            _logger.LogWarning("No filters specified for a rule; this rule will never match.");
        }

        if (_pipeline is null)
        {
            throw new InvalidOperationException("Pipeline cannot be null.");
        }

        return new ProcessingRule
        {
            Filters = [.. _filters],
            Pipeline = _pipeline,
            Condition = _useAndLogic,
            Operation = _operation,
        };
    }
}