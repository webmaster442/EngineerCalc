using System.Text.RegularExpressions;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands.Abstraction;

internal abstract class TableCommand<TDataSet> : Command<TableCommandArgs>
{
    private static string RewriteToRegex(string pattern)
    {
        return string.IsNullOrEmpty(pattern)
            ? ".*"
            : "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
    }

    public override int Execute(CommandContext context, TableCommandArgs settings)
    {
        var filter = !settings.Regex
            ? new Regex(RewriteToRegex(settings.Filter), settings.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None)
            : new Regex(settings.Filter, settings.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

        var dataSet = GetDataSet(filter);

        var table = new Table();
        table.AddColumns(GetTableHeaders());
        foreach (var data in dataSet)
        {
            table.AddRow(ToTableRow(data));
        }
        AnsiConsole.Write(table);

        return ExitCodes.Success;
    }

    protected abstract string[] ToTableRow(TDataSet data);

    protected abstract IEnumerable<TDataSet> GetDataSet(Regex filter);

    protected abstract string[] GetTableHeaders();
}
