using FileFlower.Core.FileWatchers.Contract;

namespace FileFlower.Core.Pipelines;

public class FileProcessingPipeline(IEnumerable<IFileProcessingStep> steps)
{
    private readonly IEnumerable<IFileProcessingStep> _steps = steps;

    public async Task ExecuteAsync(FileInfo file)
    {
        foreach (var step in _steps)
        {
            await step.ExecuteAsync(file);
        }
    }
}
