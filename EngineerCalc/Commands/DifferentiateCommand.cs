//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using DynamicEvaluator;

using EngineerCalc.Commands.Abstraction;
using EngineerCalc.Domain;

using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class DifferentiateCommand : ExpressionCommand<DifferentiateCommand.Settings>
{
    public sealed class Settings : ExpressionCommandSettings
    {
        [CommandOption("-v|--by-variable")]
        [Description("The variable to derive by.")]
        public string ByVariable { get; set; } = string.Empty;
    }

    public DifferentiateCommand(IEvaluatorApi api, State state) : base(api, state)
    {
    }

    protected override void ProcessExpression(IExpression expression, Settings settings)
    {
        IExpression differentiated = expression
            .Simplify()
            .Differentiate(settings.ByVariable)
            .Simplify()
            .Simplify();

        PrintResult("Differentiated expression", differentiated);
    }
}
