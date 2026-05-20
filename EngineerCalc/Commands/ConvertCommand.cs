using System.ComponentModel;

using DynamicEvaluator;
using DynamicEvaluator.TypeSystem;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class ConvertCommand : ExpressionCommand<ConvertCommand.Settings>
{
    public sealed class Settings : ExpressionCommandSettings
    {
        [Description("The unit to convert from.")]
        [CommandOption("-f|--from <UNIT>")]
        public string FromUnit { get; init; } = string.Empty;

        [Description("The unit to convert to.")]
        [CommandOption("-t|--to <UNIT>")]
        public string ToUnit { get; init; } = string.Empty;
    }

    private readonly UnitConverter _unitConverter;

    public ConvertCommand(IEvaluatorApi api, State state) : base(api, state)
    {
        _unitConverter = new UnitConverter();
    }

    protected override void ProcessExpression(IExpression expression, Settings settings)
    {
        Result value = expression.Simplify().Evaluate(_api.VariablesAndConstants);
        double inputValue = value.CastToDouble();

        double result = _unitConverter.Convert(settings.FromUnit, settings.ToUnit, inputValue);

        PrintResult($"{inputValue} {settings.FromUnit} = {result} {settings.ToUnit}");
    }
}
