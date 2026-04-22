//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Api;
using EngineerCalc.Extensions;
using EngineerCalc.Tui.Readline;

namespace EngineerCalc.Tui;

internal sealed class LineCompleter : ICompleter
{
    private readonly IEnumerable<string> _functions;
    private readonly IDictionary<string, IReadOnlyList<string>> _commands;
    private readonly IEvaluatorApi _evaluator;

    public LineCompleter(
        IEnumerable<string> functions,
        IDictionary<string, IReadOnlyList<string>> commands,
        IEvaluatorApi evaluator)
    {
        _functions = functions;
        _commands = commands;
        _evaluator = evaluator;
    }

    public IEnumerable<string> GetCompletion(string line, int currentPosition)
    {
        string[] words = line.Split(' ');
        int wordIndex = line.ToWordIndex(currentPosition);

        if (wordIndex == 0 && words[wordIndex].StartsWith('.'))
            return _commands.Where(c => c.Key.StartsWith(words[wordIndex])).Select(c => c.Key);

        if (wordIndex > 0 && words[0].StartsWith('.'))
        {
            string cmd = words[0];
            if (_commands.TryGetValue(cmd, out var args))
                return args.Where(a => a.StartsWith(words[wordIndex], StringComparison.InvariantCultureIgnoreCase));
        }

        var functions = _functions.Where(f => f.StartsWith(words[wordIndex], StringComparison.InvariantCultureIgnoreCase));
        var variables = _evaluator.VariableNames().Where(v => v.StartsWith(words[wordIndex], StringComparison.InvariantCultureIgnoreCase));

        return functions.Concat(variables).Order();
    }
}
