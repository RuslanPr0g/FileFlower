using FileFlower.Core.FileWatchers;

namespace FileFlower.Core.Extensions;

public static class FileWatcherBuilderExtensions
{
    public static FileWatcherBuilder FilterByExtension(
        this FileWatcherBuilder builder,
        string extension)
        => builder.Filter($"*.{extension.TrimStart('.')}");

    public static FileWatcherBuilder FilterMultiple(
        this FileWatcherBuilder builder,
        params string[] patterns)
    {
        foreach (var pattern in patterns)
            builder.Filter(pattern);
        return builder;
    }
}
