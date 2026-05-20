//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;

using DynamicEvaluator;
using DynamicEvaluator.TypeSystem;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;

namespace EngineerCalc.Commands;

internal sealed class DetailsCommand : ExpressionCommand<ExpressionCommandSettings>
{
    public DetailsCommand(IEvaluatorApi api, State state) : base(api, state)
    {
    }

    private static byte[] GetBytes(Result result)
    {
        return result.TypeState switch
        {
            TypeState.Boolean => BitConverter.GetBytes(result.CastToBoolean()),
            TypeState.Integer => result.CastToBigInteger().ToByteArray(),
            TypeState.Double => BitConverter.GetBytes(result.CastToDouble()),
            TypeState.String => System.Text.Encoding.UTF8.GetBytes(result.CastToString()),
            _ => throw new InvalidOperationException("Unsupported type."),
        };
    }

    protected override void ProcessExpression(IExpression expression, ExpressionCommandSettings settings)
    {
        Result result = expression.Evaluate(_api.VariablesAndConstants);
        byte[] bytes = GetBytes(result);

        Table table = new();
        for (int i = bytes.Length - 1; i >= 0; i--)
        {
            table.AddColumn($"Byte {i}");
        }
        table.AddRow(bytes.Reverse().Select(b => b.ToString("D", CultureInfo.InvariantCulture).PadLeft(9, ' ')).ToArray());
        table.AddRow(bytes.Reverse().Select(b => $"0x{b.ToString("X2", CultureInfo.InvariantCulture)}".PadLeft(9, ' ')).ToArray());
        table.AddRow(bytes.Reverse().Select(b => $"{Convert.ToString(b, 2)}b".PadLeft(9, '0')).ToArray());
        AnsiConsole.Write(table);
    }
}
