//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Extensions;
using EngineerCalc.Tui.Readline;

namespace EngineerCalc.Tui;

internal sealed class LineCompleter : ICompleter
{
    private readonly IEnumerable<string> _functions;
    private readonly IEnumerable<string> _commands;

    public LineCompleter(IEnumerable<string> functions, IEnumerable<string> commands)
    {
        _functions = functions;
        _commands = commands;
    }

    public IEnumerable<string> GetCompletion(string line, int currentPosition)
    {
        string[] words = line.Split(' ');
        int wordIndex = line.ToWordIndex(currentPosition);

        if (words[wordIndex].StartsWith('.'))
            return _commands.Where(f => f.StartsWith(words[wordIndex]));

        return _functions.Where(f => f.StartsWith(words[wordIndex]));
    }
}
