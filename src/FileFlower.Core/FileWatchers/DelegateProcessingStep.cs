using FileFlower.Core.FileWatchers.Contract;

namespace FileFlower.Core.FileWatchers;

/// <summary>
/// Represents an action that can be executed on a file.
/// </summary>
/// <param name="handler">The action to execute on a file.</param>
public sealed class DelegateProcessingStep(Func<FileInfo, Task> handler) : IFileProcessingStep
{
    private readonly Func<FileInfo, Task> _handler = handler;

    /// <summary>
    /// Executes a specified action on the given file.
    /// </summary>
    /// <param name="file">The given file to execute an action upon.</param>
    public Task ExecuteAsync(FileInfo file) => _handler(file);
}
