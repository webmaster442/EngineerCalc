using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DynamicEvaluator;

public partial class ResultBuilder
{
    private readonly StringBuilder _builder;
    private readonly CultureInfo _culture;

    [GeneratedRegex("[1-9]000", RegexOptions.Singleline, 2000)]
    private static partial Regex DigitWith3LeadingZeros();

    private static int GetDigits(IFormattable formattable)
    {
        const int maxDigits = 20;
        const int zeroDigitCount = 3;
        string[] str = formattable.ToString("N25", CultureInfo.InvariantCulture).Split('.');
        if (str.Length == 1)
            return 0;

        var match = DigitWith3LeadingZeros().Match(str[1]);
        return match.Success ? (match.Index + zeroDigitCount) : maxDigits;
    }

    private static string FormatFloat(IFormattable formattable, CultureInfo cultureInfo)
    {
        int digits = GetDigits(formattable);

        return formattable.ToString($"N{digits}", cultureInfo)
            .TrimEnd('0')
            .TrimEnd(cultureInfo.NumberFormat.NumberDecimalSeparator[0]);
    }

    public ResultBuilder(CultureInfo culture)
    {
        _builder = new StringBuilder(50);
        _culture = culture;
    }

    public ResultBuilder Append(long l)
    {
        _builder.Append(l.ToString("N0", _culture));
        return this;
    }

    public ResultBuilder Append(double d)
    {
        _builder.Append(FormatFloat(d, _culture));
        return this;
    }

    public ResultBuilder Append(float f)
    {
        _builder.Append(FormatFloat(f, _culture));
        return this;
    }

    public ResultBuilder Append(string str)
    {
        _builder.Append(str);
        return this;
    }

    public ResultBuilder AppendNewLine()
    {
        _builder.Append("\t\n");
        return this;
    }

    public override string ToString()
        => _builder.ToString();
}
