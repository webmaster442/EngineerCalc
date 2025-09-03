using System.Globalization;

using DynamicEvaluator;

using EngineerCalc;
using EngineerCalc.Api;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;

var expressionFactory = new ExpressionFactory();
var evaluatorApi = new EvaluatorApi(new VariablesAndConstantsCollection(), expressionFactory);
var commandRunnerApi = new CommandRunnerApi();

var services = new ServiceCollection();
services.AddSingleton<IApplicationApi, ApplicationApi>();
services.AddSingleton<IEvaluatorApi>(evaluatorApi);
services.AddSingleton<ICommandRunnerApi>(commandRunnerApi);

var runner = new CommandRunner(services);

await commandRunnerApi.Init(runner);

var readline = new LineReader(new LineCompleter(expressionFactory.KnownFunctions, commandRunnerApi.KnownCommands.Keys));

while (true)
{
    Prompt.DoPrompt();
    string line = readline.ReadLine("> ");
    if (string.IsNullOrWhiteSpace(line))
        continue;

    List<string> tokens = line.SplitBySpaceOrQuotes().ToList();

    try
    {
        var state =IdentifyState(commandRunnerApi.KnownCommands.Keys, tokens);
        switch (state)
        {
            case CommandState.Empty:
                continue;
            case CommandState.NotACommand:
                IExpression expression = expressionFactory.Create(line);
                dynamic result = expression.Evaluate(evaluatorApi.VariablesAndConstants);
                string resultString = ResultFormatter.Format(result, CultureInfo.InvariantCulture);
                AnsiConsole.MarkupLine(resultString);
                break;
            case CommandState.KnownCommand:
                await runner.RunAsync(tokens);
                break;
            case CommandState.UnknownCommand:
                AnsiConsole.WriteLine($"Unknown command: {tokens[0]}");
                break;
        }
        AnsiConsole.WriteLine();
    }
    catch (Exception ex)
    {
        if (ex is InvalidOperationException or OverflowException)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]{ex.Message}[/]");
            continue;
        }

#if DEBUG
        AnsiConsole.WriteException(ex);
#else
        AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace);
#endif
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