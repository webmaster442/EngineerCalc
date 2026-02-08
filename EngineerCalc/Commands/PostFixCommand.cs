//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class PostFixCommand : Command
{
    private readonly State _state;

    public PostFixCommand(State state)
    {
        _state = state;
    }

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        _state.ParseMode = ParseMode.Postfix;
        return ExitCodes.Success;
    }
}
