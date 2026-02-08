//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;

namespace EngineerCalc.Commands;

internal sealed class CommandsCommand : TableCommand<KeyValuePair<string, string>>
{
    private readonly ICommandRunnerApi _api;

    public CommandsCommand(ICommandRunnerApi api)
    {
        _api = api;
    }

    protected override IEnumerable<KeyValuePair<string, string>> GetDataSet(Regex filter)
        => _api.KnownCommands
            .Where(cmd => filter.IsMatch(cmd.Key))
            .OrderBy(cmd => cmd.Key)
            .Select(cmd => new KeyValuePair<string, string>(cmd.Key, cmd.Value.Description));

    protected override string[] GetTableHeaders()
        => ["Name", "Description"];

    protected override string[] ToTableRow(KeyValuePair<string, string> data)
        => [data.Key, data.Value];

    protected override void AfterTable()
    {
        AnsiConsole.WriteLine("Use the --help option to display command help.");
        AnsiConsole.MarkupLine("Example: [gray].details --help[/]");
    }
}
