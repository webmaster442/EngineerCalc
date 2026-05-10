//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

using EngineerCalc.Models.XmlDoc;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Api;

internal class CommandRunnerApi : ICommandRunnerApi
{
    private CommandRunner? _runner;
    private readonly HashSet<string> _allowedCommands;


    public IDictionary<string, Models.XmlDoc.Command> KnownCommands { get; }

    public CommandRunnerApi()
    {
        KnownCommands = new Dictionary<string, Models.XmlDoc.Command>();
        _allowedCommands = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            ".clear",
            ".culture",
            ".infix",
            ".postfix",
            ".simplify",
            ".unset",
            ".details"
        };
    }

    public async Task Init(CommandRunner runner)
    {
        _runner = runner;

        using var buffer = new StringWriter();
        var captureConsole = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(buffer)
        });

        runner.App.Configure(cfg => cfg.ConfigureConsole(captureConsole));

        await runner.RunAsync(["cli", "xmldoc"]);

        runner.App.Configure(cfg => cfg.ConfigureConsole(AnsiConsole.Console));

        var text = buffer.ToString();

        XmlSerializer xs = new XmlSerializer(typeof(SpectreCliCommands));

        using var reader = new StringReader(text);

        if (xs.Deserialize(reader) is SpectreCliCommands cmdRoot)
        {
            foreach (var command in cmdRoot.Commands)
            {
                KnownCommands.Add(command.Name, command);
            }
        }
    }

    public Task RunRestrictedAsync(IReadOnlyList<string> tokens)
    {
        if (_runner == null)
            throw new InvalidOperationException("CommandRunnerApi is not initialized.");

        if (_allowedCommands.Contains(tokens[0]))
        {
            return _runner.RunAsync(tokens);
        }

        throw new InvalidOperationException($"Command '{tokens[0]}' is not allowed in scripting context.");
    }
}
