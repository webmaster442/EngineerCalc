using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class ClearCommand : Command
{
    private readonly ICommandApi _api;

    public ClearCommand(ICommandApi api)
    {
        _api = api;
    }

    public override int Execute(CommandContext context)
    {
        _api.Clear();
        return ExitCodes.Success;
    }
}
