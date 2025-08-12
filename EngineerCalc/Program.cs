using DynamicEvaluator;

using EngineerCalc;
using EngineerCalc.Commands;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

var expressionFactory = new ExpressionFactory();
var api = new CommandApi(new VariablesAndConstantsCollection(), expressionFactory);

Dictionary<string, ICommand> commands = new(StringComparer.OrdinalIgnoreCase)
{
    { ".exit", new ExitCommand() },
    { ".clear", new ClearCommand() }
};

var statusBar = new Statusbar();
var readline = new LineReader(new LineCompleter(expressionFactory.KnownFunctions, commands.Keys));

while (true)
{
    statusBar.Render($"Time: {DateTime.Now}");

    string line = readline.ReadLine("> ");
    if (string.IsNullOrWhiteSpace(line))
        continue;

    IEnumerable<string> tokens = line.SplitBySpaceOrQuotes();
    string commandName = tokens.FirstOrDefault() ?? string.Empty;
    string[] arguments = [.. tokens.Skip(1)];

    try
    {
        if (commands.TryGetValue(commandName, out ICommand? command))
        {
            await command.Execute(api, arguments);
        }
        else
        {
            var expression = expressionFactory.Create(line);
            var result = expression.Evaluate(api.Evaluator.VariablesAndConstants);
            Console.WriteLine(result.ToString());
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");

    }
}