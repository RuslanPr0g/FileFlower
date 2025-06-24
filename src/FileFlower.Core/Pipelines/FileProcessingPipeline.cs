using FileFlower.Core.FileWatchers.Contract;

namespace FileFlower.Core.Pipelines;

/// <summary>
/// Represents a pipeline of asynchronous processing steps
/// to be executed sequentially on a <see cref="FileInfo"/> instance.
/// </summary>
public sealed class FileProcessingPipeline
{
    private readonly List<IFileProcessingStep> _steps = [];

    /// <summary>
    /// Adds a new asynchronous processing step to the pipeline.
    /// </summary>
    /// <param name="step">The asynchronous function that processes a <see cref="FileInfo"/>.</param>
    public void AddStep(IFileProcessingStep step)
    {
        _steps.Add(step);
    }

    /// <summary>
    /// Executes all configured processing steps sequentially on the specified <paramref name="file"/>.
    /// </summary>
    /// <param name="file">The file to process.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous execution of the pipeline.</returns>
    public async Task ExecuteAsync(FileInfo file)
    {
        foreach (var step in _steps)
        {
            await step.ExecuteAsync(file);
        }
    }
}