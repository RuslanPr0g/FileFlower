namespace FileFlower.Core.FileWatchers.Contract;

public interface IFileProcessingStep
{
    Task ExecuteAsync(FileInfo file);
}
