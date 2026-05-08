//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Extensions;

internal static class CommandStateExtensions
{
    public static CommandState IdentifyState(this IReadOnlyList<string> tokens, IEnumerable<string> commands)
    {
        if (tokens.Count == 0)
            return CommandState.Empty;

        if (tokens[0].StartsWith('.'))
        {
            return commands.Contains(tokens[0])
                ? CommandState.KnownCommand
                : CommandState.UnknownCommand;
        }

        return CommandState.NotACommand;
    }
}
