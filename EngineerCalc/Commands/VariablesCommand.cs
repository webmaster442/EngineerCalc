using System.Text.RegularExpressions;

using EngineerCalc.Commands.Abstraction;

namespace EngineerCalc.Commands;

internal sealed class VariablesCommand : TableCommand<KeyValuePair<string, object>>
{
    private readonly ICommandApi _api;

    public VariablesCommand(ICommandApi api)
    {
        _api = api;
    }

    protected override IEnumerable<KeyValuePair<string, object>> GetDataSet(Regex filter)
        => _api.Evaluator.VariablesAndConstants.Variables().Where(kv => filter.IsMatch(kv.Key));

    protected override string[] GetTableHeaders()
        => ["Name", "Value"];

    protected override string[] ToTableRow(KeyValuePair<string, object> data)
        => [data.Key, data.Value.ToString() ?? string.Empty];
}