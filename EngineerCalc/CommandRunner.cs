using EngineerCalc.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc;

internal sealed class CommandRunner
{
    public CommandRunner(ServiceCollection services)
    {
        App = new CommandApp(new TypeRegistrar(services));
        App.Configure(config =>
        {
            config
                .AddCommand<Commands.ClearCommand>(".clear")
                .WithDescription("Clears the console screen");

            config
                .AddCommand<Commands.ExitCommand>(".exit")
                .WithDescription("Exits the application");

            config
                .AddCommand<Commands.VariablesCommand>(".variables")
                .WithDescription("Lists all defined variables");

            config
                .AddCommand<Commands.CommandsCommand>(".commands")
                .WithDescription("List known commands");

            config.AddCommand<Commands.CultureCommand>(".culture")
                .WithDescription("Sets the current culture");
        });
    }

    public CommandApp App { get; }

    public async Task RunAsync(IReadOnlyList<string> tokens)
    {
        if (tokens.Count == 0)
            return;
        var result = await App.RunAsync(tokens);
        if (result != 0)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Command execution failed with code {result}.[/]");
        }
    }
}
