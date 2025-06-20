namespace FileFlower.Core.FileWatchers.Interfaces;

public interface IFileProcessingStep
{
    Task ExecuteAsync(FileInfo file);
}
