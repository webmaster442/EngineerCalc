using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class VariablesCommand : Command
{
    private readonly ICommandApi _api;

    public VariablesCommand(ICommandApi api)
    {
        _api = api;
    }
    public override int Execute(CommandContext context)
    {
        var variables =  _api.Evaluator.VariablesAndConstants;
        var table = new Table();
        table.AddColumns("Name", "Value");
        foreach (var (name, value) in variables.Variables())
        {
            string valueStr = value.ToString();
            table.AddRow(name, valueStr);
        }

        AnsiConsole.Write(table);

        return ExitCodes.Success;
    }
}