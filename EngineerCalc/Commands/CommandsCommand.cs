using System.Text.RegularExpressions;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

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
}
