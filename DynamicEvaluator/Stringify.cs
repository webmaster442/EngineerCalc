using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

using DynamicEvaluator.Types;

namespace DynamicEvaluator;

public static partial class Extensions
{
    [GeneratedRegex(@"[\""].+?[\""]|\S+")]
    private static partial Regex QuotesMatcher();

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
        ResultBuilder resultBuilder = new(cultureInfo);

        if (result is string str)
            return str;

        if (result is long l)
        {
            return resultBuilder.Append(l).AppendNewLine().Append("{{ ").Append(l).Append(" }}").ToString();
        }

        if (result is double d)
        {
            return resultBuilder.Append(d).AppendNewLine().Append("{{ ").Append(d).Append(" }}").ToString();
        }

        if (result is Vector2 vector2)
        {
            return resultBuilder
                .Append("X: ")
                .Append(vector2.X)
                .Append(", Y: ")
                .Append(vector2.Y)
                .AppendNewLine()
                .Append(@"{{\begin{pmatrix} ")
                .Append(vector2.X)
                .Append(" & ")
                .Append(vector2.Y)
                .Append(@" \end{pmatrix}}}")
                .ToString();
        }

        if (result is Vector3 vector3)
        {
            return resultBuilder
                .Append("X: ")
                .Append(vector3.X)
                .Append(", Y: ")
                .Append(vector3.Y)
                .Append(", Z: ")
                .Append(vector3.Z)
                .AppendNewLine()
                .Append(@"{{\begin{pmatrix} ")
                .Append(vector3.X)
                .Append(" & ")
                .Append(vector3.Y)
                .Append(" & ")
                .Append(vector3.Z)
                .Append(@" \end{pmatrix}}}")
                .ToString();
        }

        if (result is Vector4 vector4)
        {
            return resultBuilder
                .Append("X: ")
                .Append(vector4.X)
                .Append(", Y: ")
                .Append(vector4.Y)
                .Append(", Z: ")
                .Append(vector4.Z)
                .Append(", W: ")
                .Append(vector4.W)
                .AppendNewLine()
                .Append(@"{{\begin{pmatrix} ")
                .Append(vector4.X)
                .Append(" & ")
                .Append(vector4.Y)
                .Append(" & ")
                .Append(vector4.Z)
                .Append(" & ")
                .Append(vector4.W)
                .Append(@" \end{pmatrix}}}")
                .ToString();
        }

        if (result is Fraction fraction)
        {
            if (fraction.Denominator == 1)
                return resultBuilder.Append(fraction.Numerator).ToString();

            return resultBuilder.Append(fraction.ToString())
                .AppendNewLine()
                .Append("{{{ ")
                .Append(fraction.Numerator)
                .Append(" \\over ")
                .Append(fraction.Denominator)
                .Append("} \\sim ")
                .Append((double)fraction)
                .Append(" }}")
                .ToString();
        }

        if (result is Complex complex)
        {
            return resultBuilder
                .Append(complex.Real)
                .Append(" + i")
                .Append(complex.Imaginary)
                .AppendNewLine()
                .Append("{{ ")
                .Append(complex.Real)
                .Append(" + i")
                .Append(complex.Imaginary)
                .Append(" ~~ |Z|: ")
                .Append(complex.Magnitude)
                .Append(" ~~ \\phi: ")
                .Append(complex.Phase)
                .Append(" }}")
                .ToString();
        }

        return result.ToString() ?? "[null]";
    }
}
