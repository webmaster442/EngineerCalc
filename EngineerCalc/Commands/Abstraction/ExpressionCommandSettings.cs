//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands.Abstraction;

public class ExpressionCommandSettings : CommandSettings
{
    [CommandArgument(0, "<expression>")]
    [Description("An Expression to operate on")]
    public string Expression { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        return string.IsNullOrWhiteSpace(Expression)
            ? ValidationResult.Error("Expression cannot be empty.")
            : ValidationResult.Success();
    }
}
