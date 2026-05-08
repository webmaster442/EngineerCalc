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
