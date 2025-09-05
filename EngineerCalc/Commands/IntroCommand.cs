using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class IntroCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("EngineerCalc - A versatile engineering calculator.");
        AnsiConsole.MarkupLine("Type [bold red].commands[/] to see available commands, [bold red].exit[/] to exit");
        AnsiConsole.WriteLine();
        return ExitCodes.Success;
    }
}
