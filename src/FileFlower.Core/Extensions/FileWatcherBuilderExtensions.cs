using FileFlower.Core.FileWatchers;

namespace FileFlower.Core.Extensions;

public static class FileWatcherRuleBuilderExtensions
{
    public static FileWatcherBuilder WithOrLogic(
        this FileWatcherRuleBuilder builder)
        => builder.WithOrLogic();

    public static FileWatcherBuilder WithAndLogic(
        this FileWatcherRuleBuilder builder)
        => builder.WithAndLogic();
}
