using Microsoft.Extensions.Logging;

namespace FileFlower.Core.Loggers;

internal class ConsoleLogger<T> : ILogger<T>
{
    internal static ConsoleLogger<T> Create() => new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        Console.WriteLine("Scope began!");
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        Console.WriteLine("Logged");
    }
}
