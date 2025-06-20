using FileFlower.Core.FileWatchers;

namespace FileFlower.Core.Extensions;

public static class FileWatcherRuleBuilderExtensions
{
    public static FileWatcherRuleBuilder WithOrLogic(
        this FileWatcherRuleBuilder builder)
        => builder.UseOrLogic();

    public static FileWatcherRuleBuilder WithAndLogic(
        this FileWatcherRuleBuilder builder)
        => builder.UseAndLogic();
}
