using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class InfixCommand : Command
{
    private readonly State _state;

    public InfixCommand(State state)
    {
        _state = state;
    }

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        _state.ParseMode = ParseMode.Infix;
        return ExitCodes.Success;
    }
}
