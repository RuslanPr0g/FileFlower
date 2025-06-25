using FileFlower.Core.Abstractions;
using Microsoft.Extensions.Hosting;

namespace FileFlower.DependencyInjection;

/// <summary>
/// Provides a hosted background service that starts and stops a configured <see cref="IFileWatcher"/> instance
/// within a generic host environment.
/// </summary>
public class FileWatcherHostedService : IHostedService
{
    private readonly IEnumerable<IFileWatcher> _watchers;

    /// <summary>
    /// Initializes a new instance of <see cref="FileWatcherHostedService"/> with the specified <paramref name="watchers"/>.
    /// </summary>
    /// <param name="watchers">The file watcher instance to manage.</param>
    public FileWatcherHostedService(IEnumerable<IFileWatcher> watchers)
    {
        _watchers = watchers;
    }

    /// <summary>
    /// Starts the hosted service and begins watching for file changes.
    /// </summary>
    /// <param name="cancellationToken">A token to signal cancellation.</param>
    /// <returns>A <see cref="Task"/> that completes when the service has started.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var watcher in _watchers)
        {
            watcher.Start();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the hosted service and disposes of the underlying file watcher.
    /// </summary>
    /// <param name="cancellationToken">A token to signal cancellation.</param>
    /// <returns>A <see cref="Task"/> that completes when the service has stopped.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var watcher in _watchers)
        {
            watcher.Dispose();
        }

        return Task.CompletedTask;
    }
}
