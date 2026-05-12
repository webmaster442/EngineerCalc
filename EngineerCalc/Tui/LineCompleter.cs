//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Api;
using EngineerCalc.Extensions;
using EngineerCalc.Models;
using EngineerCalc.Models.XmlDoc;
using EngineerCalc.Tui.Readline;

namespace EngineerCalc.Tui;

internal sealed class LineCompleter : ICompleter
{
    private readonly IEnumerable<string> _functions;
    private readonly IFileSystem _fileSystem;
    private readonly State _state;
    private readonly IDictionary<string, Command> _commands;
    private readonly IEvaluatorApi _evaluator;

    private readonly string _filePathTypeName;
    private readonly string _directoryPathTypeName;

    public LineCompleter(
        IEnumerable<string> functions,
        IDictionary<string, Models.XmlDoc.Command> commands,
        IEvaluatorApi evaluator,
        IFileSystem fileSystem,
        State state)
    {
        _functions = functions;
        _fileSystem = fileSystem;
        _state = state;
        _commands = commands;
        _evaluator = evaluator;

        _filePathTypeName = typeof(FilePath).FullName!;
        _directoryPathTypeName = typeof(DirectoryPath).FullName!;
    }

    public IEnumerable<string> GetCompletion(string line, int currentPosition)
    {
        string[] words = line.Split(' ');
        int wordIndex = line.ToWordIndex(currentPosition);

        if (wordIndex == 0 && words[wordIndex].StartsWith('.'))
        {
            return CompleteCommandName(words[wordIndex]);
        }

        if (wordIndex > 0 && words[0].StartsWith('.'))
        {
            return CompleteCommandArguments(words[0], words, wordIndex);
        }

        var functions = _functions.Where(f => f.StartsWith(words[wordIndex], StringComparison.InvariantCultureIgnoreCase));
        var variables = _evaluator.VariableNames().Where(v => v.StartsWith(words[wordIndex], StringComparison.InvariantCultureIgnoreCase));

        return functions.Concat(variables).Order();
    }

    private IEnumerable<string> CompleteCommandArguments(string commandName, string[] words, int wordIndex)
    {
        if (_commands.TryGetValue(commandName, out Command? commandModel))
        {
            int correctedWordindex = commandModel.MapWordIndexAsArgsIndex(words, wordIndex);

            if ((!words[wordIndex].StartsWith('-') && commandModel.Parameters.Arguments.Any(p => p.ClrType == _filePathTypeName && p.Position == correctedWordindex))
                || (correctedWordindex < words.Length && IsFilePathSwitch(commandModel, words[correctedWordindex])))
            {
                return GetFileNames(words[wordIndex]);
            }
            else if ((!words[wordIndex].StartsWith('-') && commandModel.Parameters.Arguments.Any(p => p.ClrType == _directoryPathTypeName && p.Position == correctedWordindex))
                || (correctedWordindex < words.Length && IsDirectoryPathSwitch(commandModel, words[correctedWordindex])))
            {
                return GetDirectoryNames(words[wordIndex]);
            }
            else
            {
                var switches = commandModel.Parameters.Options.Select(p => $"-{p.Short}").Concat(commandModel.Parameters.Options.Select(p => $"--{p.Long}"));
                return switches.Where(s => s.StartsWith(words[wordIndex], StringComparison.InvariantCultureIgnoreCase));
            }
        }
        return Enumerable.Empty<string>();
    }

    private bool IsDirectoryPathSwitch(Command commandModel, string str)
    {
        var param = commandModel.Parameters.Options
            .FirstOrDefault(p => string.Equals(p.Short, str.TrimStart('-'), StringComparison.InvariantCultureIgnoreCase)
                            || string.Equals(p.Long, str.TrimStart('-'), StringComparison.InvariantCultureIgnoreCase));

        return param != null && param.ClrType == _directoryPathTypeName;
    }

    private bool IsFilePathSwitch(Command commandModel, string str)
    {
        var param = commandModel.Parameters.Options
            .FirstOrDefault(p => string.Equals(p.Short, str.TrimStart('-'), StringComparison.InvariantCultureIgnoreCase)
                            || string.Equals(p.Long, str.TrimStart('-'), StringComparison.InvariantCultureIgnoreCase));

        return param != null && param.ClrType == _filePathTypeName;
    }

    private IEnumerable<string> GetFileNames(string str)
    {
        var dataset = _fileSystem.GetFileNames(_state.CurrentDirectory);

        if (!string.IsNullOrWhiteSpace(str))
            return dataset.Where(f => f.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase)).Select(f => f.Name);

        return dataset.Select(f => f.Name);
    }

    private IEnumerable<string> GetDirectoryNames(string str)
    {
        var dataset = _fileSystem.GetDirectoryNames(_state.CurrentDirectory);

        if (!string.IsNullOrWhiteSpace(str))
            return dataset.Where(f => f.Name.StartsWith(str, StringComparison.InvariantCultureIgnoreCase)).Select(f => f.Name);

        return dataset.Select(f => f.Name);
    }

    private IEnumerable<string> CompleteCommandName(string commandName)
        => _commands.Where(c => c.Key.StartsWith(commandName)).Select(c => c.Key);
}
