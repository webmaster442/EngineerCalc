using System.ComponentModel;

using Spectre.Console.Cli;

namespace EngineerCalc.Commands.Abstraction;

internal class TableCommandArgs : CommandSettings
{
    [CommandOption("-f|--filter <FILTER>")]
    [Description("Filter pattern (supports * and ? wildcards unless --regex is used)")]
    public string Filter { get; set; }

    [CommandOption("-r|--regex")]
    [Description("Interpret filter as a regular expression")]
    public bool Regex { get; set; }

    [CommandOption("-i|--ignore-case")]
    [Description("Make filter case insensitive")]
    public bool IgnoreCase { get; set; }

    public TableCommandArgs()
    {
        Filter = string.Empty;
        Regex = false;
        IgnoreCase = false;
    }
}
