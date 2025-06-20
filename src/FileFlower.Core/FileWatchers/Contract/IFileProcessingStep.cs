namespace FileFlower.Core.FileWatchers.Contract;

/// <summary>
/// Defines a contract for a processing step that operates asynchronously on a file.
/// </summary>
public interface IFileProcessingStep
{
    /// <summary>
    /// Executes the processing logic asynchronously on the specified <paramref name="file"/>.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> instance representing the file to process.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task ExecuteAsync(FileInfo file);
}
