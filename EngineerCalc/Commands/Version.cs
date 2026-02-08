//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal class VersionCommand : Command
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var assemblyVersion = typeof(VersionCommand).Assembly.GetName().Version;
        if (assemblyVersion == null)
        {
            AnsiConsole.MarkupLine("[red]Failed to get version[/]");
            return ExitCodes.GeneralError;
        }

        AnsiConsole.MarkupLineInterpolated($"[blue]{assemblyVersion}[/]");
        return ExitCodes.Success;
    }
}
