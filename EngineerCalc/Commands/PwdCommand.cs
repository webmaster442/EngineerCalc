//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
