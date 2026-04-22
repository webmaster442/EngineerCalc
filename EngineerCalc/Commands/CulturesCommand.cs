//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;

using EngineerCalc.Commands.Abstraction;

namespace EngineerCalc.Commands;

internal sealed class CulturesCommand : TableCommand<KeyValuePair<string, string>>
{
    protected override IEnumerable<KeyValuePair<string, string>> GetDataSet(Regex filter)
    {
        return CultureInfo.GetCultures(CultureTypes.NeutralCultures)
            .OrderBy(x => x.Name)
            .Where(culture => filter.IsMatch(culture.Name))
            .Select(x => new KeyValuePair<string, string>(x.Name, x.DisplayName));
    }

    protected override string[] GetTableHeaders()
        => ["Name", "Display Name"];

    protected override string[] ToTableRow(KeyValuePair<string, string> data)
        => [data.Key, data.Value];
}

