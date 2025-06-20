using FileFlower.Core.FileWatchers.Contract;

namespace FileFlower.Core.FileWatchers;

internal class DelegateProcessingStep(Func<FileInfo, Task> handler) : IFileProcessingStep
{
    private readonly Func<FileInfo, Task> _handler = handler;

    public Task ExecuteAsync(FileInfo file) => _handler(file);
}
