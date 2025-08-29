using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class ExitCommand : Command
{
    private readonly ICommandApi _api;

    public ExitCommand(ICommandApi api)
    {
        _api = api;
    }

    public override int Execute(CommandContext context)
    {
        _api.Exit(0);
        return ExitCodes.Success;
    }
}
