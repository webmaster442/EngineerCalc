//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Tui;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class IntroCommand : Command
{
    private static Version GetVersion()
        => typeof(IntroCommand).Assembly.GetName().Version ?? new Version(1, 0);

    private static void CenteredText(string @string)
    {
        var padding = ((Console.WindowWidth - @string.Length) / 2) + 1;
        AnsiConsole.Write(new string(' ', padding));
        AnsiConsole.MarkupLine(@string);
    }

    protected override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        var banner = FigletTextFactory.AnsiShadow("EngineerCalc");
        banner.Justify(Justify.Center);
        AnsiConsole.Write(banner);
        CenteredText($"A versatile engineering calculator. [yellow italic]Version: {GetVersion()}[/]");
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("Type [bold red].commands[/] to see available commands, [bold red].exit[/] to exit");
        AnsiConsole.MarkupLine("For supported functions type [blue].functions[/]");
        AnsiConsole.Write(new Rule());
        AnsiConsole.WriteLine();
        return ExitCodes.Success;
    }
}
