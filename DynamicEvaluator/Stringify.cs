using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

using DynamicEvaluator.Types;

namespace DynamicEvaluator;

public static partial class Extensions
{
    [GeneratedRegex(@"[\""].+?[\""]|\S+")]
    private static partial Regex QuotesMatcher();

    [GeneratedRegex("[1-9]000", RegexOptions.Singleline, 2000)]
    private static partial Regex DigitWith3LeadingZeros();

    public static IEnumerable<string> SplitBySpaceOrQuotes(this string input)
    {
        var matches = QuotesMatcher().Matches(input);
        foreach (Match match in matches)
        {
            string value = match.Value;
            if ((value.StartsWith('"') && value.EndsWith('"')) ||
                (value.StartsWith('\'') && value.EndsWith('\'')))
            {
                yield return value.Substring(1, value.Length - 2);
            }
            else
            {
                yield return value;
            }
        }
    }

    public static string Stringify(this object result, CultureInfo cultureInfo)
    {
        if (result is string str)
            return str;

        if (result is Vector2 vector2)
            return $"X: {FormatFloat(vector2.X, cultureInfo)}, Y: {FormatFloat(vector2.Y, cultureInfo)}";
        
        if (result is Vector3 vector3)
            return $"X: {FormatFloat(vector3.X, cultureInfo)}, Y: {FormatFloat(vector3.Y, cultureInfo)}, Z: {FormatFloat(vector3.Z, cultureInfo)}";

        if (result is Vector4 vector4)
            return $"X: {FormatFloat(vector4.X, cultureInfo)}, Y: {FormatFloat(vector4.Y, cultureInfo)}, Z: {FormatFloat(vector4.Z, cultureInfo)}, W: {FormatFloat(vector4.W, cultureInfo)}";

        if (result is Fraction fraction)
            return FormatFraction(fraction, cultureInfo);

        if (result is Complex complex)
            return FormatComplex(complex, cultureInfo);

        if (result is IFormattable formattable)
        {
            if (IsInteger(formattable))
                return formattable.ToString("N0", cultureInfo);

            if (IsFloat(formattable))
                return FormatFloat(formattable, cultureInfo);
        }

        return result.ToString() ?? "[null]";
    }

    private static string FormatFraction(Fraction fraction, CultureInfo cultureInfo)
    {
        if (fraction.Denominator == 1)
            return fraction.Numerator.ToString(cultureInfo);

        return $"{{{{ {fraction.Numerator.ToString(cultureInfo)} \\over {fraction.Denominator.ToString(cultureInfo)} }} \\sim {Stringify((double)fraction, cultureInfo)} }}";
    }

    private static string FormatComplex(Complex complex, CultureInfo cultureInfo)
    {
        return $"""
            Real: {FormatFloat(complex.Real, cultureInfo)} Imaginary: {FormatFloat(complex.Imaginary, cultureInfo)}
            Phase: {FormatFloat(complex.Phase, cultureInfo)} Magnitude: {FormatFloat(complex.Magnitude, cultureInfo)}
            """;
    }

    private static bool IsFloat(object value)
        => value is double;

    private static bool IsInteger(object value)
        => value is long;

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
}
