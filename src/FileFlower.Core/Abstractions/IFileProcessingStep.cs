namespace FileFlower.Core.Abstractions;

/// <summary>
/// Defines a contract for a processing step that operates asynchronously on a file.
/// </summary>
public interface IFileProcessingStep
{
    /// <summary>
    /// Executes the processing logic asynchronously on the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The <see cref="FileContext"/> instance representing the file to process.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task ExecuteAsync(FileContext context);
}
