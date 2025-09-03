using System.Xml.Serialization;

using EngineerCalc.Models.XmlDoc;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Api;

internal class CommandRunnerApi : ICommandRunnerApi
{
    public IDictionary<string, Models.XmlDoc.Command> KnownCommands { get; }

    public CommandRunnerApi()
    {
        KnownCommands = new Dictionary<string, Models.XmlDoc.Command>();
    }

    public async Task Init(CommandRunner runner)
    {
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
}