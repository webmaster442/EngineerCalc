//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace EngineerCalc.DomainServices;

internal record class LogEntry
{
    public required LogLevel LogLevel { get; init; }
    public required string Message { get; init; }
    public required string CategoryName { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required string? ExceptionDetails { get; internal set; }
}
