using System.Globalization;
using System.Numerics;

using DynamicEvaluator;
using DynamicEvaluator.Types;

using Spectre.Console;

namespace EngineerCalc.Tui;
internal static class ResultFormatter
{
    private static string PropertyName(string name)
        => $"[bold silver]{name.EscapeMarkup()}[/]";

    public static string Format(object result, CultureInfo cultureInfo)
    {
        ResultBuilder resultBuilder = new(cultureInfo);

        if (result is string str)
        {
            resultBuilder
                .Append("([fuchsia]string[/]) ")
                .Append("[italic green]")
                .Append(str.EscapeMarkup())
                .Append("[/]");
        }
        else if (result is long l)
        {
            resultBuilder
                .Append("([fuchsia]long[/]) ")
                .Append("[italic green]")
                .Append(l)
                .Append("[/]");
        }
        else if (result is int i)
        {
            resultBuilder
                .Append("([fuchsia]int[/]) ")
                .Append("[italic green]")
                .Append(i)
                .Append("[/]");
        }
        else if (result is double d)
        {
            resultBuilder
                .Append("([fuchsia]double[/]) ")
                .Append("[italic green]")
                .Append(d)
                .Append("[/]");
        }
        else if (result is Fraction fraction)
        {
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
        else if (result is Complex complex)
        {
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
        else if (result is Vector2 vector2)
        {
            resultBuilder
                .Append("([fuchsia]Vector2[/]) ")
                .Append("[italic green]")
                .Append(PropertyName("X: "))
                .Append(vector2.X)
                .Append(PropertyName(" Y: "))
                .Append(vector2.Y)
                .Append("[/]");
        }
        else if (result is Vector3 vector3)
        {
            resultBuilder
                .Append("([fuchsia]Vector3[/]) ")
                .Append("[italic green]")
                .Append(PropertyName("X: "))
                .Append(vector3.X)
                .Append(PropertyName(" Y: "))
                .Append(vector3.Y)
                .Append(PropertyName(" Z: "))
                .Append(vector3.Z)
                .Append("[/]");
        }
        else if (result is Vector4 vector4)
        {
            resultBuilder
                .Append("([fuchsia]Vector4[/]) ")
                .Append("[italic green]")
                .Append(PropertyName("X: "))
                .Append(vector4.X)
                .Append(PropertyName(" Y: "))
                .Append(vector4.Y)
                .Append(PropertyName(" Z: "))
                .Append(vector4.Z)
                .Append(PropertyName(" W: "))
                .Append(vector4.W)
                .Append("[/]");
        }
        else if (result is NoResult)
        { 
        }
        else
        {
            resultBuilder
                .Append($"([fuchsia]{result.GetType()}[/]) ")
                .Append("[italic green]")
                .Append(result.ToString() ?? string.Empty)
                .Append("[/]");
        }

        return resultBuilder.ToString();
    }
}
