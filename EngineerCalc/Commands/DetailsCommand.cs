using System.ComponentModel;
using System.Globalization;

using EngineerCalc.Api;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal class DetailsCommand : Command<DetailsCommand.Settings>
{
    private readonly IEvaluatorApi _api;
    private readonly State _state;

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<expression>")]
        [Description("The expression to evaluate.")]
        public string Expression { get; set; } = string.Empty;

        public override ValidationResult Validate()
        {
            return string.IsNullOrWhiteSpace(Expression)
                ? ValidationResult.Error("Expression cannot be empty.") 
                : ValidationResult.Success();
        }
    }

    public DetailsCommand(IEvaluatorApi api, State state)
    {
        _api = api;
        _state = state;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        try
        {
            dynamic result = _state.ParseMode == ParseMode.Infix
                ? _api.Parse(settings.Expression).Evaluate(_api.VariablesAndConstants)
                : _api.ParseRpn(settings.Expression).Evaluate(_api.VariablesAndConstants);

            byte[] bytes = GetBytes(result);

            Table table = new();
            for (int i = bytes.Length-1; i >= 0; i--)
            {
                table.AddColumn($"Byte {i}");
            }
            table.AddRow(bytes.Reverse().Select(b => b.ToString("D", CultureInfo.InvariantCulture).PadLeft(9, ' ')).ToArray());
            table.AddRow(bytes.Reverse().Select(b => $"0x{b.ToString("X2", CultureInfo.InvariantCulture)}".PadLeft(9, ' ')).ToArray());
            table.AddRow(bytes.Reverse().Select(b => $"{Convert.ToString(b, 2)}b".PadLeft(9, '0')).ToArray());
            AnsiConsole.Write(table);

        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error:[/] {ex.Message}[/]");
            return ExitCodes.GeneralError;
        }


        return ExitCodes.Success;
    }

    private byte[] GetBytes(dynamic result)
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
}
