using System.ComponentModel;

using EngineerCalc.Api;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class UnsetCommand : Command<UnsetCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("Name of the variable to unset")]
        public string Name { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return ValidationResult.Error("Name cannot be empty");

            return ValidationResult.Success();
        }
    }


    private readonly IEvaluatorApi _api;

    public UnsetCommand(IEvaluatorApi api)
    {
        _api = api;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (_api.VariablesAndConstants.IsConstant(settings.Name))
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Cannot unset constant '{settings.Name}'.[/]");
            return ExitCodes.GeneralError;
        }

        if (!_api.VariablesAndConstants.IsVariable(settings.Name))
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Variable '{settings.Name}' does not exist.[/]");
            return ExitCodes.GeneralError;
        }

        _api.VariablesAndConstants.Remove(settings.Name);
        return ExitCodes.Success;
    }
}