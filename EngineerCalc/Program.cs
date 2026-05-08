//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator;

using EngineerCalc;
using EngineerCalc.Api;
using EngineerCalc.Extensions;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;

using Webmaster442.WindowsTerminal;

var appState = new State();
var expressionFactory = new ExpressionFactory();
var evaluatorApi = new EvaluatorApi(new VariablesAndConstantsCollection(), expressionFactory, appState);
var commandRunnerApi = new CommandRunnerApi();

var services = new ServiceCollection();
services.AddSingleton(appState);
services.AddSingleton<IApplicationApi, ApplicationApi>();
services.AddSingleton<IEvaluatorApi>(evaluatorApi);
services.AddSingleton<ICommandRunnerApi>(commandRunnerApi);
services.AddSingleton<IFileSystem, FileSystem>();

Console.OutputEncoding = System.Text.Encoding.UTF8;

var runner = new CommandRunner(services);

await commandRunnerApi.Init(runner);

var lineCompleter = new LineCompleter(expressionFactory.KnownFunctions, commandRunnerApi.GetAutocompleteData(), evaluatorApi);
var readline = new LineReader(lineCompleter);

AnsiConsole.Clear();

await runner.RunAsync([".intro"]);

while (true)
{
    Terminal.ShellIntegration.StartOfPrompt();
    Prompt.DoPrompt(appState);
    Terminal.ShellIntegration.CommandStart();
    string line = readline.ReadLine(" ─> ");
    Terminal.ShellIntegration.CommandExecuted();
    if (string.IsNullOrWhiteSpace(line))
        continue;

    List<string> tokens = line.SplitBySpaceOrQuotes().ToList();

    try
    {
        switch (tokens.IdentifyState(commandRunnerApi.KnownCommands.Keys))
        {
            case CommandState.Empty:
                continue;
            case CommandState.NotACommand:
                ScriptFileRunner.EvaluateExpression(appState, expressionFactory, evaluatorApi, line);
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
        AnsiConsole.WriteException(ex, ExceptionFormats.Default);
#else
        AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace);
#endif
    }
    finally
    {
        AnsiConsole.WriteLine();
    }
}
