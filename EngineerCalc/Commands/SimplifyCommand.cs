using DynamicEvaluator;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;

namespace EngineerCalc.Commands;

internal sealed class SimplifyCommand : ExpressionCommand
{
    public SimplifyCommand(IEvaluatorApi api, State state) : base(api, state)
    {
    }

    protected override void ProcessExpression(IExpression expression)
    {
        IExpression simplified;
        if (expression.TrySimplfyAsLogicExpression(out IExpression? simpleLogicExpression))
        {
            simplified = simpleLogicExpression;
        }
        else
        {
            simplified = expression
                .Simplify()
                .Simplify();
        }
        AnsiConsole.MarkupLineInterpolated($"[green]Simplified expression: {simplified}[/]");
    }
}
