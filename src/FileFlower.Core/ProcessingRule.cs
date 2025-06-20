using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Pipelines;

namespace FileFlower.Core;

public enum ProcessingRuleCondition
{
    And = 0,
    Or = 1,
}

public sealed class ProcessingRule
{
    public required List<IFileFilter> Filters { get; init; }
    public required FileProcessingPipeline Pipeline { get; init; }
    public ProcessingRuleCondition Condition { get; init; } = ProcessingRuleCondition.And;

    public async Task<bool> TryProcessAsync(FileInfo file)
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
}
