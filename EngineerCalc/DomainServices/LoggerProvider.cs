using EngineerCalc.Domain;

using Microsoft.Extensions.Logging;

namespace EngineerCalc.DomainServices;

internal sealed class LoggerProvider : ILoggerProvider
{
    private readonly RingBuffer<LogEntry> _logEntries;
    private readonly Dictionary<string, Logger> _loggers;
    private readonly TimeProvider _timeProvider;

    public LoggerProvider(int capacity, TimeProvider timeProvider)
    {
        _logEntries = new RingBuffer<LogEntry>(capacity);
        _loggers = new Dictionary<string, Logger>();
        _timeProvider = timeProvider;
    }

    public List<LogEntry> GetLogEntries()
        => _logEntries.GetValues();

    public ILogger CreateLogger(string categoryName)
    {
        if (_loggers.TryGetValue(categoryName, out var logger))
        {
            return logger;
        }
        logger = new Logger(categoryName, _logEntries, _timeProvider);
        _loggers[categoryName] = logger;
        return logger;
    }

    public void Dispose()
    {
        _loggers.Clear();
        _logEntries.Clear();
    }
}
