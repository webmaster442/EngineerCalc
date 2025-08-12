using DynamicEvaluator;

using EngineerCalc;
using EngineerCalc.Commands;
using EngineerCalc.Tui;
using EngineerCalc.Tui.Readline;

var readline = new LineReader();
var expressionFactory = new ExpressionFactory();
var api = new CommandApi(new VariablesAndConstantsCollection(), expressionFactory);
var statusBar = new Statusbar();

Dictionary<string, ICommand> commands = new(StringComparer.OrdinalIgnoreCase)
{
    { ".exit", new ExitCommand() },
    { ".clear", new ClearCommand() }
};

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