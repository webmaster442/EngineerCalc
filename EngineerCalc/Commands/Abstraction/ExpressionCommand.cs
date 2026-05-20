//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator;

using EngineerCalc.Api;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands.Abstraction;

internal abstract class ExpressionCommand<TSettings> : Command<TSettings>
    where TSettings: ExpressionCommandSettings
{
    protected readonly IEvaluatorApi _api;
    protected readonly State _state;

    public ExpressionCommand(IEvaluatorApi api, State state)
    {
        _api = api;
        _state = state;
    }

    protected override int Execute(CommandContext context, TSettings settings, CancellationToken cancellationToken)
    {
        try
        {
            IExpression expression = _state.ParseMode == ParseMode.Infix
                ? _api.Parse(settings.Expression)
                : _api.ParseRpn(settings.Expression);

            ProcessExpression(expression, settings);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error: {ex.Message}[/]");
            return ExitCodes.GeneralError;
        }

        return ExitCodes.Success;
    }

    protected abstract void ProcessExpression(IExpression expression, TSettings settings);

    protected static void PrintResult(string header, IExpression result)
    {
        AnsiConsole.MarkupLineInterpolated($"[green]{header.EscapeMarkup()}: {result}[/]");
    }

    protected static void PrintResult(FormattableString str)
    {
        AnsiConsole.MarkupLineInterpolated($"[green]{str}[/]");
    }
}
