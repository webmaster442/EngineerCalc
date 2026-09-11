using EngineerCalc.Domain;

using Microsoft.Extensions.Logging;

namespace EngineerCalc.DomainServices;

internal sealed class Logger : ILogger
{
    private readonly string _categoryName;
    private readonly RingBuffer<LogEntry> _logEntries;
    private readonly TimeProvider _timeProvider;

    public Logger(string categoryName, RingBuffer<LogEntry> logEntries, TimeProvider timeProvider)
    {
        _categoryName = categoryName;
        _logEntries = logEntries;
        _timeProvider = timeProvider;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        _logEntries.Add(new LogEntry
        {
            CategoryName = _categoryName,
            LogLevel = logLevel,
            Message = formatter(state, exception),
            Timestamp = _timeProvider.GetUtcNow(),
            ExceptionDetails = FormatException(exception)
        });
    }

    private static string? FormatException(Exception? exception)
    {
        return exception == null
            ? null
            : $"""
               {exception.GetType()}
               {exception.StackTrace}
               """;
    }
}
