namespace FileFlower.Core.Pipelines;

public sealed class FileProcessingPipeline
{
    private readonly List<Func<FileInfo, Task>> _steps = [];

    public void AddStep(Func<FileInfo, Task> step)
    {
        _steps.Add(step);
    }

    public async Task ExecuteAsync(FileInfo file)
    {
        foreach (var step in _steps)
        {
            await step(file);
        }
    }
}