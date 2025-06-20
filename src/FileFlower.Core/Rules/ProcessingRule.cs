using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Pipelines;

namespace FileFlower.Core.Rules;

/// <summary>
/// Specifies logical conditions used to combine multiple file filters in a processing rule.
/// </summary>
public enum ProcessingRuleCondition : byte
{
    /// <summary>All filters must match (logical AND).</summary>
    And = 0,

    /// <summary>At least one filter must match (logical OR).</summary>
    Or = 1,
}

/// <summary>
/// Specifies the file system operations that a processing rule can respond to.
/// </summary>
public enum ProcessingRuleOperation : byte
{
    /// <summary>Operation is not specified.</summary>
    NotSpecified = 0,

    /// <summary>File was created.</summary>
    Created = 1,

    /// <summary>File was changed.</summary>
    Changed = 2,

    /// <summary>File was deleted.</summary>
    Deleted = 3,

    /// <summary>File was renamed.</summary>
    Renamed = 4,
}

/// <summary>
/// Represents a rule for processing files that includes filters, pipeline steps,
/// the logical condition for combining filters, and the file operation it applies to.
/// </summary>
public sealed class ProcessingRule
{
    /// <summary>
    /// Gets the collection of file filters to evaluate against files.
    /// </summary>
    public required List<IFileFilter> Filters { get; init; }

    /// <summary>
    /// Gets the pipeline of processing steps to execute on matching files.
    /// </summary>
    public required FileProcessingPipeline Pipeline { get; init; }

    /// <summary>
    /// Gets the logical condition (AND/OR) used to combine the filters.
    /// </summary>
    public ProcessingRuleCondition Condition { get; init; } = ProcessingRuleCondition.And;

    /// <summary>
    /// Gets the file system operation that this rule applies to.
    /// </summary>
    public ProcessingRuleOperation Operation { get; init; } = ProcessingRuleOperation.NotSpecified;

    /// <summary>
    /// Attempts to process the specified <paramref name="file"/> if the operation is Created
    /// and the filters match.
    /// </summary>
    /// <param name="file">The file to process.</param>
    /// <returns>A <see cref="Task{Boolean}"/> indicating if processing was executed.</returns>
    public async Task<bool> TryProcessCreatedAsync(FileInfo file)
    {
        if (!IsValidOperation(ProcessingRuleOperation.Created))
        {
            return false;
        }

        return await TryProcessAsync(file);
    }

    /// <summary>
    /// Attempts to process the specified <paramref name="file"/> if the operation is Changed
    /// and the filters match.
    /// </summary>
    /// <param name="file">The file to process.</param>
    /// <returns>A <see cref="Task{Boolean}"/> indicating if processing was executed.</returns>
    public async Task<bool> TryProcessChangedAsync(FileInfo file)
    {
        if (!IsValidOperation(ProcessingRuleOperation.Changed))
        {
            return false;
        }

        return await TryProcessAsync(file);
    }

    /// <summary>
    /// Attempts to process the specified <paramref name="file"/> if the operation is Renamed
    /// and the filters match.
    /// </summary>
    /// <param name="file">The file to process.</param>
    /// <returns>A <see cref="Task{Boolean}"/> indicating if processing was executed.</returns>
    public async Task<bool> TryProcessRenamedAsync(FileInfo file)
    {
        if (!IsValidOperation(ProcessingRuleOperation.Renamed))
        {
            return false;
        }

        return await TryProcessAsync(file);
    }

    /// <summary>
    /// Attempts to process the specified <paramref name="file"/> if the operation is Deleted
    /// and the filters match.
    /// </summary>
    /// <param name="file">The file to process.</param>
    /// <returns>A <see cref="Task{Boolean}"/> indicating if processing was executed.</returns>
    public async Task<bool> TryProcessDeletedAsync(FileInfo file)
    {
        if (!IsValidOperation(ProcessingRuleOperation.Deleted))
        {
            return false;
        }

        return await TryProcessAsync(file);
    }

    private async Task<bool> TryProcessAsync(FileInfo file)
    {
        bool matches = Condition.Equals(ProcessingRuleCondition.And)
                    ? Filters.TrueForAll(f => f.Matches(file))
                    : Filters.Exists(f => f.Matches(file));

        if (matches)
        {
            await Pipeline.ExecuteAsync(file);
            return true;
        }

        return false;
    }

    private bool IsValidOperation(ProcessingRuleOperation operation)
    {
        return operation.Equals(Operation);
    }
}
