using DynamicEvaluator;

using EngineerCalc.Api;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands.Abstraction;

internal abstract class ExpressionCommand : Command<ExpressionCommandSettings>
{
    protected readonly IEvaluatorApi _api;
    protected readonly State _state;

    public ExpressionCommand(IEvaluatorApi api, State state)
    {
        _api = api;
        _state = state;
    }

    public override int Execute(CommandContext context, ExpressionCommandSettings settings)
    {
        try
        {
            IExpression expression = _state.ParseMode == ParseMode.Infix
                ? _api.Parse(settings.Expression)
                : _api.ParseRpn(settings.Expression);

            ProcessExpression(expression);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error: {ex.Message}[/]");
            return ExitCodes.GeneralError;
        }

        return ExitCodes.Success;
    }

    protected abstract void ProcessExpression(IExpression expression);
}
