using FileFlower.Core.Abstractions;
using FileFlower.Core.Filters;
using FileFlower.Core.Loggers;
using FileFlower.Core.Pipelines;
using FileFlower.Core.ProcessingSteps;
using Microsoft.Extensions.Logging;

namespace FileFlower.Core.Rules;

/// <summary>
/// Builds and configures <see cref="ProcessingRule"/> instances,
/// allowing fluent addition of filters, steps, and logical conditions.
/// </summary>
public sealed class FileWatcherRuleBuilder
{
    private readonly List<IFileFilter> _filters = [];
    private readonly FileProcessingPipeline _pipeline = new();
    private ProcessingRuleCondition _useAndLogic = ProcessingRuleCondition.And;
    private FileModificationType _operation = FileModificationType.NotSpecified;

    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="FileWatcherRuleBuilder"/>
    /// with the specified <paramref name="logger"/> for diagnostics.
    /// </summary>
    /// <param name="logger">The logger to use for warnings and informational messages.</param>
    public FileWatcherRuleBuilder(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="FileWatcherRuleBuilder"/>
    /// with a default console logger.
    /// </summary>
    public FileWatcherRuleBuilder()
    {
        _logger = ConsoleLogger.Create();
    }

    /// <summary>
    /// Adds a file name filter pattern to the rule.
    /// </summary>
    /// <param name="pattern">The glob-style pattern to match file names (e.g., "*.txt").</param>
    /// <returns>The current <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public FileWatcherRuleBuilder Filter(string pattern)
    {
        Filter(new FileNameFilter(pattern));
        return this;
    }

    /// <summary>
    /// Adds a custom defined filter to the rule.
    /// </summary>
    /// <param name="filter">User defined custom filter.</param>
    /// <returns>The current <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public FileWatcherRuleBuilder Filter<TFileFilter>(TFileFilter filter)
        where TFileFilter : IFileFilter
    {
        _filters.Add(filter);
        return this;
    }

    /// <summary>
    /// Sets the filter combination logic to logical OR.
    /// </summary>
    /// <returns>The current <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public FileWatcherRuleBuilder UseOrLogic()
    {
        _useAndLogic = ProcessingRuleCondition.Or;
        return this;
    }

    /// <summary>
    /// Sets the filter combination logic to logical AND (default).
    /// </summary>
    /// <returns>The current <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public FileWatcherRuleBuilder UseAndLogic()
    {
        _useAndLogic = ProcessingRuleCondition.And;
        return this;
    }

    /// <summary>
    /// Adds an asynchronous processing step to the rule's pipeline.
    /// </summary>
    /// <param name="step">The asynchronous step to execute on matching files.</param>
    /// <returns>The current <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public FileWatcherRuleBuilder AddStep(Func<FileContext, Task> step)
    {
        AddStep<DelegateProcessingStep>(new DelegateProcessingStep(step));
        return this;
    }

    /// <summary>
    /// Adds a custom asynchronous processing step to the rule's pipeline.
    /// </summary>
    /// <param name="step">The asynchronous step to execute on matching files.</param>
    /// <returns>The current <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public FileWatcherRuleBuilder AddStep<TFileProcessingStep>(IFileProcessingStep step)
        where TFileProcessingStep : IFileProcessingStep
    {
        _pipeline.AddStep(step);
        return this;
    }

    internal ProcessingRule BuildWithOperationCreated()
    {
        return Build(FileModificationType.Created);
    }

    internal ProcessingRule BuildWithOperationChanged()
    {
        return Build(FileModificationType.Changed);
    }

    internal ProcessingRule BuildWithOperationDeleted()
    {
        return Build(FileModificationType.Deleted);
    }

    internal ProcessingRule BuildWithOperationRenamed()
    {
        return Build(FileModificationType.Renamed);
    }

    internal ProcessingRule Build(FileModificationType operation)
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