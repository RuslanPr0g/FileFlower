using FileFlower.Core.FileWatchers.Contract;
using Microsoft.Extensions.Hosting;

namespace FileFlower.DependencyInjection;

public class FileWatcherHostedService : IHostedService
{
    private readonly IFileWatcher _watcher;

    public FileWatcherHostedService(IFileWatcher watcher)
    {
        _watcher = watcher;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _watcher.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher.Dispose();
        return Task.CompletedTask;
    }
}
