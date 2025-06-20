using FileFlower.Core.FileWatchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FileFlower.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileWatcher(
        this IServiceCollection services,
        string path,
        Action<FileWatcherBuilder> configure)
    {
        services.AddSingleton(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<FileWatcher>>();
            var builder = new FileWatcherBuilder(path, logger);
            configure(builder);
            return builder.Start();
        });

        return services;
    }
}
