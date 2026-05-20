//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using DynamicEvaluator;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class SimplifyCommand : ExpressionCommand<ExpressionCommandSettings>
{
    public SimplifyCommand(IEvaluatorApi api, State state) : base(api, state)
    {
    }

    protected override void ProcessExpression(IExpression expression, ExpressionCommandSettings settings)
    {
        IExpression simplified = expression.TrySimplfyAsLogicExpression(out IExpression? simpleLogicExpression)
            ? simpleLogicExpression
            : expression
                .Simplify()
                .Simplify();

        PrintResult("Simplified expression", simplified);
    }
}
