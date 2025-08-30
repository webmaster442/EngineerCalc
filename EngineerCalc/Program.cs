using System.Globalization;

using DynamicEvaluator;

using EngineerCalc;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

using Spectre.Console;

var expressionFactory = new ExpressionFactory();
var api = new CommandApi(new VariablesAndConstantsCollection(), expressionFactory);

var runner = new CommandRunner(api);
var readline = new LineReader(new LineCompleter(expressionFactory.KnownFunctions, runner.KnownCommands));

while (true)
{
    Prompt.DoPrompt();
    string line = readline.ReadLine("> ");
    if (string.IsNullOrWhiteSpace(line))
        continue;

    List<string> tokens = line.SplitBySpaceOrQuotes().ToList();

    try
    {

        var state = runner.IdentifyState(tokens);
        switch (state)
        {
            case CommandState.Empty:
                continue;
            case CommandState.NotACommand:
                IExpression expression = expressionFactory.Create(line);
                dynamic result = expression.Evaluate(api.Evaluator.VariablesAndConstants);
                string resultString = ResultFormatter.Format(result, CultureInfo.InvariantCulture);
                AnsiConsole.MarkupLine(resultString);
                break;
            case CommandState.KnownCommand:
                await runner.Run(tokens);
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