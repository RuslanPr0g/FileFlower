using FileFlower.Core.Abstractions;
using FileFlower.Core.Pipelines;

namespace FileFlower.Core.Rules;

/// <summary>
/// Specifies logical conditions used to combine multiple context filters in a processing rule.
/// </summary>
public enum ProcessingRuleCondition : byte
{
    /// <summary>All filters must match (logical AND).</summary>
    And = 0,

    /// <summary>At least one filter must match (logical OR).</summary>
    Or = 1,
}

/// <summary>
/// Represents a rule for processing files that includes filters, pipeline steps,
/// the logical condition for combining filters, and the context operation it applies to.
/// </summary>
public sealed class ProcessingRule
{
    /// <summary>
    /// Gets the collection of context filters to evaluate against files.
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
    /// Gets the context system operation that this rule applies to.
    /// </summary>
    public FileModificationType Operation { get; init; } = FileModificationType.NotSpecified;

    /// <summary>
    /// Attempts to process the specified <paramref name="context"/>
    /// where the filters match.
    /// </summary>
    /// <param name="context">The context to process.</param>
    /// <returns>A <see cref="Task{Boolean}"/> indicating if processing was executed.</returns>
    public async Task<bool> TryProcessAsync(FileContext context)
    {
        if (!IsValidOperation(context.FileModificationType))
        {
            return false;
        }

        return await InternalTryProcessAsync(context);
    }

    private async Task<bool> InternalTryProcessAsync(FileContext context)
    {
        bool matches = Condition.Equals(ProcessingRuleCondition.And)
                    ? Filters.TrueForAll(f => f.Matches(context))
                    : Filters.Exists(f => f.Matches(context));

        if (matches)
        {
            await Pipeline.ExecuteAsync(context);
            return true;
        }

        return false;
    }

    private bool IsValidOperation(FileModificationType operation)
    {
        return operation.Equals(Operation);
    }
}
