//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator;
using DynamicEvaluator.Types;

using EngineerCalc;
using EngineerCalc.Api;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;

using Webmaster442.WindowsTerminal;

var appState = new State();
var expressionFactory = new ExpressionFactory();
var evaluatorApi = new EvaluatorApi(new VariablesAndConstantsCollection(), expressionFactory);
var commandRunnerApi = new CommandRunnerApi();

var services = new ServiceCollection();
services.AddSingleton(appState);
services.AddSingleton<IApplicationApi, ApplicationApi>();
services.AddSingleton<IEvaluatorApi>(evaluatorApi);
services.AddSingleton<ICommandRunnerApi>(commandRunnerApi);

var runner = new CommandRunner(services);

await commandRunnerApi.Init(runner);

var readline = new LineReader(new LineCompleter(expressionFactory.KnownFunctions, commandRunnerApi.GetAutocompleteData()));

AnsiConsole.Clear();

await runner.RunAsync([".intro"]);

while (true)
{
    Terminal.ShellIntegration.StartOfPrompt();
    Prompt.DoPrompt(appState);
    Terminal.ShellIntegration.CommandStart();
    string line = readline.ReadLine("╚═> ");
    Terminal.ShellIntegration.CommandExecuted();
    if (string.IsNullOrWhiteSpace(line))
        continue;

    List<string> tokens = line.SplitBySpaceOrQuotes().ToList();

    try
    {
        switch (IdentifyState(commandRunnerApi.KnownCommands.Keys, tokens))
        {
            case CommandState.Empty:
                continue;
            case CommandState.NotACommand:
                EvaluateExpression(appState, expressionFactory, evaluatorApi, line);
                break;
            case CommandState.KnownCommand:
                await runner.RunAsync(tokens);
                break;
            case CommandState.UnknownCommand:
                AnsiConsole.WriteLine($"Unknown command: {tokens[0]}");
                break;
        }
        Terminal.ShellIntegration.CommandFinished(0);
    }
    catch (Exception ex)
    {
        if (ex is InvalidOperationException or OverflowException)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]{ex.Message}[/]");
            Terminal.ShellIntegration.CommandFinished(-1);
            continue;
        }

#if DEBUG
        AnsiConsole.WriteException(ex);
#else
        AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace);
#endif
    }
    finally
    {
        AnsiConsole.WriteLine();
    }
}

static CommandState IdentifyState(IEnumerable<string> commands, IReadOnlyList<string> tokens)
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

static void EvaluateExpression(State appState,
                               ExpressionFactory expressionFactory,
                               EvaluatorApi evaluatorApi,
                               string line)
{
    IExpression expression = appState.ParseMode switch
    {
        ParseMode.Infix => expressionFactory.Create(line),
        ParseMode.Postfix => expressionFactory.CreateFromRpn(line),
        _ => throw new InvalidOperationException("Unknown parse mode."),
    };
    dynamic result = expression.Evaluate(evaluatorApi.VariablesAndConstants);

    if (result is not NoResult)
        evaluatorApi.VariablesAndConstants["ans"] = result;

    string resultString = ResultFormatter.Format(result, appState.Culture);
    AnsiConsole.MarkupLine(resultString);
}
