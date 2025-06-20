namespace FileFlower.Core.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="FileWatcherRuleBuilder"/> class,
/// facilitating fluent configuration of filter logic with explicit AND/OR semantics.
/// </summary>
public static class FileWatcherRuleBuilderExtensions
{
    /// <summary>
    /// Configures the builder to combine filters using logical OR semantics.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FileWatcherRuleBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public static FileWatcherRuleBuilder WithOrLogic(
        this FileWatcherRuleBuilder builder)
        => builder.UseOrLogic();

    /// <summary>
    /// Configures the builder to combine filters using logical AND semantics.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FileWatcherRuleBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="FileWatcherRuleBuilder"/> instance for chaining.</returns>
    public static FileWatcherRuleBuilder WithAndLogic(
        this FileWatcherRuleBuilder builder)
        => builder.UseAndLogic();
}
