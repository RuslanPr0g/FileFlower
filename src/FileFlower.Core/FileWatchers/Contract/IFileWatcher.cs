namespace FileFlower.Core.FileWatchers.Contract;

/// <summary>
/// Defines the contract for a file watcher capable of monitoring a file system location
/// and triggering processing logic, with the ability to be started and disposed.
/// </summary>
public interface IFileWatcher : IDisposable
{
    /// <summary>
    /// Starts the file watcher, enabling it to monitor the target directory and process matching files.
    /// </summary>
    void Start();
}
