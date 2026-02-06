using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal class KeysCommand : Command
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var table = new Table();
        table.AddColumns("Key", "Description");
        table.AddRow("Up arrow", "Recall previous command from history");
        table.AddRow("Down arrow", "Recall next command from history");
        table.AddRow("Tab", "Next autocomplete item");
        table.AddRow("Shift+Tab", "Previous autocomplete item");
        AnsiConsole.Write(table);

        return ExitCodes.Success;
    }
}
