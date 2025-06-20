using FileFlower.Core.FileWatchers.Contract;
using FileFlower.Core.Pipelines;

namespace FileFlower.Core;

public enum ProcessingRuleCondition : byte
{
    And = 0,
    Or = 1,
}

public enum ProcessingRuleOperation : byte
{
    NotSpecified = 0,
    Created = 1,
    Changed = 2,
    Deleted = 3,
    Renamed = 4,
}

public sealed class ProcessingRule
{
    public required List<IFileFilter> Filters { get; init; }
    public required FileProcessingPipeline Pipeline { get; init; }
    public ProcessingRuleCondition Condition { get; init; } = ProcessingRuleCondition.And;
    public ProcessingRuleOperation Operation { get; init; } = ProcessingRuleOperation.NotSpecified;

    public async Task<bool> TryProcessCreatedAsync(FileInfo file)
    {
        if (!IsValidOperation(ProcessingRuleOperation.Created))
        {
            return false;
        }

        return await TryProcessAsync(file);
    }

    public async Task<bool> TryProcessChangedAsync(FileInfo file)
    {
        if (!IsValidOperation(ProcessingRuleOperation.Changed))
        {
            return false;
        }

        return await TryProcessAsync(file);
    }

    public async Task<bool> TryProcessRenamedAsync(FileInfo file)
    {
        if (!IsValidOperation(ProcessingRuleOperation.Renamed))
        {
            return false;
        }

        return await TryProcessAsync(file);
    }

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
