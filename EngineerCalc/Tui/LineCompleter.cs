//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Domain;
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
        SplitPathInput(str, out string searchDirectory, out string prefixToKeep, out string namePrefix);

        if (!Directory.Exists(searchDirectory))
            return [];

        var dataset = _fileSystem.GetFileNames(searchDirectory);

        if (!string.IsNullOrEmpty(namePrefix))
            return dataset
                .Where(f => f.Name.StartsWith(namePrefix, StringComparison.InvariantCultureIgnoreCase))
                .Select(f => prefixToKeep + f.Name);

        return dataset.Select(f => prefixToKeep + f.Name);
    }

    private void SplitPathInput(string str, out string searchDirectory, out string prefixToKeep, out string namePrefix)
    {
        if (string.IsNullOrEmpty(str))
        {
            searchDirectory = _state.CurrentDirectory;
            prefixToKeep = string.Empty;
            namePrefix = string.Empty;
            return;
        }

        bool endsWithSeparator = str[^1] == Path.DirectorySeparatorChar || str[^1] == Path.AltDirectorySeparatorChar;

        string dirPart;
        if (endsWithSeparator)
        {
            dirPart = str;
            namePrefix = string.Empty;
            prefixToKeep = str;
        }
        else
        {
            dirPart = Path.GetDirectoryName(str) ?? string.Empty;
            namePrefix = Path.GetFileName(str);
            prefixToKeep = dirPart.Length == 0
                ? string.Empty
                : (str[..^namePrefix.Length]);
        }

        if (string.IsNullOrEmpty(dirPart))
        {
            searchDirectory = _state.CurrentDirectory;
        }
        else if (Path.IsPathRooted(dirPart))
        {
            searchDirectory = dirPart;
        }
        else
        {
            searchDirectory = Path.GetFullPath(dirPart, _state.CurrentDirectory);
        }
    }

    private IEnumerable<string> GetDirectoryNames(string str)
    {
        SplitPathInput(str, out string searchDirectory, out string prefixToKeep, out string namePrefix);

        if (!Directory.Exists(searchDirectory))
            return [];

        var dataset = _fileSystem.GetDirectoryNames(searchDirectory);

        if (!string.IsNullOrEmpty(namePrefix))
            return dataset
                .Where(f => f.Name.StartsWith(namePrefix, StringComparison.InvariantCultureIgnoreCase))
                .Select(f => prefixToKeep + f.Name);

        return dataset.Select(f => prefixToKeep + f.Name);
    }

    private IEnumerable<string> CompleteCommandName(string commandName)
        => _commands.Where(c => c.Key.StartsWith(commandName)).Select(c => c.Key);
}
