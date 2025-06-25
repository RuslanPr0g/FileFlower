using FileFlower.Core.Abstractions;

namespace FileFlower.Core.ProcessingSteps;

/// <summary>
/// Represents an action that can be executed on a file.
/// </summary>
/// <param name="handler">The action to execute on a file.</param>
public sealed class DelegateProcessingStep(Func<FileContext, Task> handler) : IFileProcessingStep
{
    private readonly Func<FileContext, Task> _handler = handler;

    /// <summary>
    /// Executes a specified action on the given file.
    /// </summary>
    /// <param name="context">The given file to execute an action upon.</param>
    public Task ExecuteAsync(FileContext context) => _handler(context);
}
