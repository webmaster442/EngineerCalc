using DynamicEvaluator;

using EngineerCalc;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

using Spectre.Console;

var expressionFactory = new ExpressionFactory();
var api = new CommandApi(new VariablesAndConstantsCollection(), expressionFactory);

var runner = new CommandRunner(api);
var statusBar = new Statusbar();
var readline = new LineReader(new LineCompleter(expressionFactory.KnownFunctions, runner.KnownCommands));

while (true)
{
    statusBar.Render($"Time: {DateTime.Now}");

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
                var expression = expressionFactory.Create(line);
                var result = expression.Evaluate(api.Evaluator.VariablesAndConstants);
                AnsiConsole.WriteLine(result.ToString());
                break;
            case CommandState.KnownCommand:
                await runner.Run(tokens);
                break;
            case CommandState.UnknownCommand:
                AnsiConsole.WriteLine($"Unknown command: {tokens[0]}");
                break;
        }
    }
    catch (Exception ex)
    {
        if (ex is InvalidOperationException)
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