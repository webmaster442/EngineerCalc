using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class PwdCommand : Command
{
    private readonly State _state;

    public PwdCommand(State state)
    {
        _state = state;
    }

    protected override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine(_state.CurrentDirectory);
        return ExitCodes.Success;
    }
}
