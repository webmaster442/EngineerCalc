//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using DynamicEvaluator;
using DynamicEvaluator.TypeSystem;

using EngineerCalc.Api;
using EngineerCalc.Extensions;
using EngineerCalc.Tui;

using Spectre.Console;

namespace EngineerCalc;

internal sealed class ScriptFileRunner
{
    private readonly ExpressionFactory _expressionFactory;
    private readonly IEvaluatorApi _evaluator;
    private readonly State _state;
    private readonly ICommandRunnerApi _commandRunner;
    private readonly IFileSystem _fileSystem;

    public static void EvaluateExpression(State appState,
                                          ExpressionFactory expressionFactory,
                                          IEvaluatorApi evaluatorApi,
                                          string line)
    {
        IExpression expression = appState.ParseMode switch
        {
            ParseMode.Infix => expressionFactory.Create(line),
            ParseMode.Postfix => expressionFactory.CreateFromRpn(line),
            _ => throw new InvalidOperationException("Unknown parse mode."),
        };
        Result result = expression.Evaluate(evaluatorApi.VariablesAndConstants);

        if (result.TypeState != TypeState.NoResult)
        {
            evaluatorApi.VariablesAndConstants["ans"] = result;
            string resultString = ResultFormatter.Format(result, appState.Culture);
            AnsiConsole.MarkupLine(resultString);
        }
    }

    public ScriptFileRunner(ExpressionFactory expressionFactory,
                            IEvaluatorApi evaluator,
                            State state,
                            ICommandRunnerApi commandRunner,
                            IFileSystem fileSystem)
    {
        _expressionFactory = expressionFactory;
        _evaluator = evaluator;
        _state = state;
        _commandRunner = commandRunner;
        _fileSystem = fileSystem;
    }

    public async Task<bool> RunAsync(string file)
    {
        using StreamReader reader = new(_fileSystem.OpenRead(file), Encoding.UTF8);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                continue;
            }

            List<string> tokens = line.SplitBySpaceOrQuotes().ToList();

            try
            {
                switch (tokens.IdentifyState(_commandRunner.KnownCommands.Keys))
                {
                    case CommandState.Empty:
                        continue;
                    case CommandState.NotACommand:
                        ScriptFileRunner.EvaluateExpression(_state, _expressionFactory, _evaluator, line);
                        break;
                    case CommandState.KnownCommand:
                        await _commandRunner.RunRestrictedAsync(tokens);
                        break;
                    case CommandState.UnknownCommand:
                        throw new InvalidOperationException($"Invalid command: {tokens[0]}");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        return true;
    }
}
