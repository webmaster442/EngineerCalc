//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class IntroCommand : Command
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine("EngineerCalc - A versatile engineering calculator.");
        AnsiConsole.MarkupLine("Type [bold red].commands[/] to see available commands, [bold red].exit[/] to exit");
        AnsiConsole.MarkupLine("For supported functions type [blue].functions[/]");
        AnsiConsole.Write(new Rule());
        AnsiConsole.WriteLine();
        return ExitCodes.Success;
    }
}
