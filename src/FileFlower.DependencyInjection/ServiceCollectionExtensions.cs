using FileFlower.Core.FileWatchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FileFlower.DependencyInjection;

/// <summary>
/// Extension methods to register <see cref="FileWatcher"/> and
/// its hosted service into an <see cref="IServiceCollection"/> for dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a <see cref="FileWatcher"/> instance configured for the specified directory path
    /// and configuration action, as a singleton service.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="path">The directory path to watch.</param>
    /// <param name="configure">An action to configure the <see cref="FileWatcherBuilder"/>.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance for chaining.</returns>
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
            return builder.Build();
        });

        return services;
    }

    /// <summary>
    /// Registers the <see cref="FileWatcherHostedService"/> as a hosted background service.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection AddFileWatcherHostedService(this IServiceCollection services)
    {
        services.AddHostedService<FileWatcherHostedService>();
        return services;
    }
}
