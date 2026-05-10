//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using DynamicEvaluator.TypeSystem;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

namespace EngineerCalc.Commands;

internal sealed class VariablesCommand : TableCommand<KeyValuePair<string, Result>>
{
    private readonly IEvaluatorApi _api;

    public VariablesCommand(IEvaluatorApi api)
    {
        _api = api;
    }

    protected override IEnumerable<KeyValuePair<string, Result>> GetDataSet(Regex filter)
        => _api.VariablesAndConstants.Variables().Where(kv => filter.IsMatch(kv.Key));

    protected override string[] GetTableHeaders()
        => ["Name", "Value", "Type"];

    protected override string[] ToTableRow(KeyValuePair<string, Result> data)
        => [data.Key, data.Value.ToString(), data.Value.TypeState.ToString()];
}
