using EngineerCalc.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc;

internal sealed class CommandRunner
{
    private readonly CommandApp _commandApp;
    private readonly HashSet<string> _knownCommands;
    private readonly ServiceCollection _services;

    public CommandRunner(ICommandApi api)
    {
        _services = new ServiceCollection();
        _services.AddSingleton(api);
        _commandApp = new CommandApp(new TypeRegistrar(_services));
        _commandApp.Configure(config =>
        {
            config
                .AddCommand<Commands.ClearCommand>(".clear")
                .WithDescription("Clears the console screen.");
            config
                .AddCommand<Commands.ExitCommand>(".exit")
                .WithDescription("Exits the application.");

        });
        _knownCommands = [".clear", ".exit"];
    }

    public IEnumerable<string> KnownCommands => _knownCommands;

    public async Task Run(IReadOnlyList<string> tokens)
    {
        if (tokens.Count == 0)
            return;
        var result = await _commandApp.RunAsync(tokens);
        if (result != 0)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Command execution failed with code {result}.[/]");
        }
    }

    public bool CanRun(IReadOnlyList<string> tokens)
    {
        return tokens.Count != 0
            && _knownCommands.Contains(tokens[0]);
    }
}
