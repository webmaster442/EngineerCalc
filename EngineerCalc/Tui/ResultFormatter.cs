//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;

using DynamicEvaluator;
using DynamicEvaluator.TypeSystem;

using Spectre.Console;

namespace EngineerCalc.Tui;

internal static class ResultFormatter
{
    private static string PropertyName(string name)
        => $"[bold silver]{name.EscapeMarkup()}[/]";

    public static string Format(Result result, CultureInfo cultureInfo)
    {
        ResultBuilder resultBuilder = new(cultureInfo);

        switch (result.TypeState)
        {
            case TypeState.String:
                {
                    resultBuilder
                        .Append("([fuchsia]string[/]) ")
                        .Append("[italic green]")
                        .Append(result.CastToString().EscapeMarkup())
                        .Append("[/]");
                }
                break;
            case TypeState.Integer:
                {
                    resultBuilder
                        .Append("([fuchsia]integer[/]) ")
                        .Append("[italic green]")
                        .Append(result.CastToBigInteger())
                        .Append("[/]");
                }
                break;
            case TypeState.Double:
                {
                    resultBuilder
                        .Append("([fuchsia]double[/]) ")
                        .Append("[italic green]")
                        .Append(result.CastToDouble())
                        .Append("[/]");
                }
                break;
            case TypeState.Fraction:
                {
                    var fraction = result.CastToFraction();
                    resultBuilder
                        .Append("([fuchsia]Fraction[/]) ")
                        .Append("[italic green]")
                        .Append(fraction.Numerator)
                        .Append(" / ")
                        .Append(fraction.Denominator)
                        .Append(" ~ ")
                        .Append((double)fraction)
                        .Append("[/]");
                }
                break;
            case TypeState.NoResult:
                break;
            case TypeState.Boolean:
                {
                    resultBuilder
                        .Append("([fuchsia]string[/]) ")
                        .Append("[italic green]")
                        .Append(result.CastToBoolean())
                        .Append("[/]");
                }
                break;
            case TypeState.Complex:
                {
                    var complex = result.CastToComplex();
                    resultBuilder
                        .Append("([fuchsia]Complex[/]) ")
                        .Append("[italic green]")
                        .Append(complex.Real)
                        .Append(" + ")
                        .Append(complex.Imaginary)
                        .Append("i ")
                        .Append(PropertyName("|Z|: "))
                        .Append(complex.Magnitude)
                        .Append(PropertyName(" Phi:"))
                        .Append(complex.Phase)
                        .Append("[/]");
                }
                break;
            case TypeState.Array:
                {
                    var numberArray = result.CastToArray();
                    resultBuilder
                        .Append("([fuchsia]NumberArray[/]) ")
                        .Append("[italic green]")
                        .Append($"[{string.Join(", ", numberArray)}]".EscapeMarkup())
                        .Append("[/]");
                }
                break;
        }

        return resultBuilder.ToString();
    }
}
