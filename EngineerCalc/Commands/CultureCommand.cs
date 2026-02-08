//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class CultureCommand : Command<CultureCommand.Settings>
{
    private readonly State _state;

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<culture>")]
        public string Culture { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Culture))
                return ValidationResult.Error("Culture cannot be empty.");

            return ValidationResult.Success();
        }
    }

    public CultureCommand(State state)
    {
        _state = state;
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            CultureInfo culture = new CultureInfo(settings.Culture);
            _state.Culture = culture;
            return ExitCodes.Success;
        }
        catch (CultureNotFoundException)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Culture '{settings.Culture}' is not valid.[/]");
            return ExitCodes.GeneralError;
        }

    }
}
