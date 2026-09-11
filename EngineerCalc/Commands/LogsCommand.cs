using System;
using System.Collections.Generic;
using System.Text;

using EngineerCalc.DomainServices;

using Microsoft.Extensions.Logging;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class LogsCommand : Command
{
    private readonly LoggerProvider _loggerProvider;

    public LogsCommand(LoggerProvider loggerProvider)
    {
        _loggerProvider = loggerProvider;
    }

    protected override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var entries =_loggerProvider.GetLogEntries();
        foreach (var entry in entries)
        {
            AnsiConsole.MarkupInterpolated($"[bold]{entry.Timestamp:HH:mm:ss}[/] ");
            switch (entry.LogLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Trace:
                    AnsiConsole.Markup("[italic gray]Trace[/] ");
                    break;
                case LogLevel.Information:
                    AnsiConsole.Markup("[italic blue]Information[/] ");
                    break;
                case LogLevel.Warning:
                    AnsiConsole.Markup("[italic yellow]Warning[/] ");
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    AnsiConsole.Markup("[italic red]Error[/] ");
                    break;
                case LogLevel.None:
                    AnsiConsole.Markup("[italic]None[/] ");
                    break;
            }
            AnsiConsole.WriteLine(entry.Message);
            if (entry.ExceptionDetails is not null)
            {
                AnsiConsole.MarkupLine($"[italic gray]{entry.ExceptionDetails.EscapeMarkup()}[/]");    
            }
        }

        return ExitCodes.Success;
    }
}
