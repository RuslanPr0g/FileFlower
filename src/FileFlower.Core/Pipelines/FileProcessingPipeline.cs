using FileFlower.Core.Abstractions;

namespace FileFlower.Core.Pipelines;

/// <summary>
/// Represents a pipeline of asynchronous processing steps
/// to be executed sequentially on a <see cref="FileContext"/> instance.
/// </summary>
public sealed class FileProcessingPipeline
{
    private readonly List<IFileProcessingStep> _steps = [];

    /// <summary>
    /// Adds a new asynchronous processing step to the pipeline.
    /// </summary>
    /// <param name="step">The asynchronous function that processes a <see cref="FileContext"/>.</param>
    public void AddStep(IFileProcessingStep step)
    {
        _steps.Add(step);
    }

    /// <summary>
    /// Executes all configured processing steps sequentially on the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The file to process.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous execution of the pipeline.</returns>
    public async Task ExecuteAsync(FileContext context)
    {
        foreach (var step in _steps)
        {
            await step.ExecuteAsync(context);
        }
    }
}