using DynamicEvaluator;

namespace EngineerCalc.Endpoints;

internal sealed class EndpointFunctions
{
    private readonly ExpressionFactory _expressionFactory;
    private readonly Dictionary<string, ICommand> _commands;
    private readonly Version _version;

    public EndpointFunctions()
    {
        _expressionFactory = new ExpressionFactory();
        _commands = LoadCommands();
        _version = typeof(EndpointFunctions).Assembly.GetName().Version ?? new Version(0, 0);
    }

    private static Dictionary<string, ICommand> LoadCommands()
    {
        static ICommand CreateInstance(Type commandType)
        {
            return Activator.CreateInstance(commandType) as ICommand
                ?? throw new InvalidOperationException($"{commandType} construction failed");
        }

        Dictionary<string, ICommand> results = new();
        var commandTypes = typeof(EndpointFunctions).Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ICommand)))
            .Where(t => !t.IsInterface && !t.IsAbstract);

        foreach (var commandType in commandTypes)
        {
            var cmd = CreateInstance(commandType);
            string name = cmd.Name.StartsWith('#') ? cmd.Name : $"#{cmd.Name}";
            results.Add(name, cmd);
        }

        return results;
    }

    public async Task<Result> RunCommand(State state, string cmdline)
    {
        HtmlBuilder htmlBuilder = new();
        var tokens = cmdline.ToLower().SplitBySpaceOrQuotes();
        var cmd = tokens.FirstOrDefault() ?? "";
        var args = tokens.Skip(1).ToArray();
        try
        {
            if (string.IsNullOrEmpty(cmd))
                return Result.FromSuccess("");

            if (!_commands.ContainsKey(cmd))
            {
                return Result.FromError($"Unknown command: {cmd}");
            }

            return await _commands[cmd].Execute(state, args);
        }
        catch (Exception ex)
        {
            return Result.FromError(htmlBuilder.Reset().Exception(ex));
        }
    }

    public Task<Result> Evaluate(State state, string expression)
    {
        return Task.Run(() =>
        {
            HtmlBuilder htmlBuilder = new();
            try
            {
                IExpression parsed = _expressionFactory.Create(expression);
                object result = parsed.Simplify().Evaluate(state!.Variables);
                htmlBuilder.AddResult(result.Stringify(state.Culture));
                return Result.FromSuccess(htmlBuilder);
            }
            catch (Exception ex)
            {
                return Result.FromError(htmlBuilder.Reset().Exception(ex));
            }
        });
    }

    public Task<Result> Intro()
    {
        string html = $"""
        <table>
            <tr>
                <td><img src="android-chrome-192x192.png" height="128" /></td>
                <td><h1 style="font-size: 48pt">Engineers calculator</h1></td>
            </tr>
        </table>
        <h3>Version: {_version}</h3>
        <ul>
            <li>To start evaluating enter an expression like: <a href="#" onclick="typeIntoInput(event)">33+22</a></li>
            <li>To list available # (hashmark) commands, type <a href="#" onclick="typeIntoInput(event)">#commands</a></li>
            <li>To clear the current output screen type: <a href="#" onclick="typeIntoInput(event)">#clear</a></li>
            <li>To reload page type: <a href="#" onclick="typeIntoInput(event)">#reload</a></li>
        </ul>
        """;

        return Task.FromResult(Result.FromSuccess(html));
    }
}
