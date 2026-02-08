using System.Globalization;

using DynamicEvaluator;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;

using Spectre.Console;

namespace EngineerCalc.Commands;

internal sealed class DetailsCommand : ExpressionCommand
{
    public DetailsCommand(IEvaluatorApi api, State state) : base(api, state)
    {
    }

    private static byte[] GetBytes(dynamic result)
    {
        if (result is float f)
            return BitConverter.GetBytes(f);
        if (result is double d)
            return BitConverter.GetBytes(d);
        if (result is decimal m)
            return decimal.GetBits(m).SelectMany(BitConverter.GetBytes).ToArray();
        if (result is int i)
            return BitConverter.GetBytes(i);
        if (result is long l)
            return BitConverter.GetBytes(l);
        if (result is short s)
            return BitConverter.GetBytes(s);
        if (result is byte b)
            return new[] { b };
        if (result is uint ui)
            return BitConverter.GetBytes(ui);
        if (result is ulong ul)
            return BitConverter.GetBytes(ul);
        if (result is ushort us)
            return BitConverter.GetBytes(us);
        if (result is sbyte sb)
            return new[] { (byte)sb };
        if (result is char c)
            return BitConverter.GetBytes(c);
        if (result is bool bo)
            return BitConverter.GetBytes(bo);
        if (result is string str)
            return System.Text.Encoding.UTF8.GetBytes(str);

        throw new InvalidOperationException("Unsupported type.");
    }

    protected override void ProcessExpression(IExpression expression)
    {
        dynamic result = expression.Evaluate(_api.VariablesAndConstants);
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
