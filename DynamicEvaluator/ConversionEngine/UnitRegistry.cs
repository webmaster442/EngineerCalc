//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace DynamicEvaluator.ConversionEngine;

internal static class UnitRegistry
{
    private static RatioUnit MakeUnit(decimal ratio, Dimension dimension, params string[] names)
    {
        return new RatioUnit
        {
            Dimension = dimension,
            RatioToBaseUnit = ratio,
            Names = new HashSet<string>(names, StringComparer.InvariantCultureIgnoreCase)
        };
    }

    public static IEnumerable<UnitBase> Units
    {
        get
        {
            //mass
            yield return MakeUnit(1m, Dimension.Mass, "g", "gram", "grams");
            yield return MakeUnit(1000m, Dimension.Mass, "kg", "kilogram", "kilograms");
            yield return MakeUnit(10m, Dimension.Mass, "dkg", "decagram", "decagrams");
            yield return MakeUnit(1000000m, Dimension.Mass, "t", "tonne", "tonnes");
            yield return MakeUnit(0.001m, Dimension.Mass, "mg", "milligram", "milligrams");
            yield return MakeUnit(453.59237m, Dimension.Mass, "lb", "pound", "pounds");
            yield return MakeUnit(28.349523125m, Dimension.Mass, "oz", "ounce", "ounces");
            yield return MakeUnit(0.2m, Dimension.Mass, "ct", "karat", "carats");
            yield return MakeUnit(6350.293m, Dimension.Mass, "stone", "stones");
            //length
            yield return MakeUnit(1m, Dimension.Length, "m", "meter", "meters");
            yield return MakeUnit(0.01m, Dimension.Length, "cm", "centimeter", "centimeters");
            yield return MakeUnit(0.001m, Dimension.Length, "mm", "millimeter", "millimeters");
            yield return MakeUnit(1000m, Dimension.Length, "km", "kilometer", "kilometers");
            yield return MakeUnit(0.0254m, Dimension.Length, "in", "inch", "inches");
            yield return MakeUnit(0.3048m, Dimension.Length, "ft", "foot", "feet");
            yield return MakeUnit(0.9144m, Dimension.Length, "yd", "yard", "yards");
            //time
            yield return MakeUnit(1m, Dimension.Time, "s", "second", "seconds");
            yield return MakeUnit(60m, Dimension.Time, "min", "minute", "minutes");
            yield return MakeUnit(3600m, Dimension.Time, "h", "hour", "hours");
            yield return MakeUnit(86400m, Dimension.Time, "d", "day", "days");
            yield return MakeUnit(604800m, Dimension.Time, "wk", "week", "weeks");
            yield return MakeUnit(0.001m, Dimension.Time, "ms", "millisecond", "milliseconds");
            //volume
            yield return MakeUnit(1m, Dimension.Volume, "l", "liter", "liters");
            yield return MakeUnit(0.001m, Dimension.Volume, "ml", "milliliter", "milliliters");
            yield return MakeUnit(0.004929m, Dimension.Volume, "tsp", "teaspoon", "teaspoons");
            yield return MakeUnit(0.014787m, Dimension.Volume, "tbsp", "tablespoon", "tablespoons");
            yield return MakeUnit(0.236588m, Dimension.Volume, "cup", "cups");
            yield return MakeUnit(0.473176m, Dimension.Volume, "pt", "pint", "pints");
            yield return MakeUnit(0.946353m, Dimension.Volume, "qt", "quart", "quarts");
            yield return MakeUnit(3.78541m, Dimension.Volume, "gal", "gallon", "gallons");
            //area
            yield return MakeUnit(1m, Dimension.Area, "m2", "square meter", "square meters");
            yield return MakeUnit(0.0001m, Dimension.Area, "cm2", "square centimeter", "square centimeters");
            yield return MakeUnit(0.000001m, Dimension.Area, "mm2", "square millimeter", "square millimeters");
            yield return MakeUnit(10000m, Dimension.Area, "ha", "hectare", "hectares");
            yield return MakeUnit(1000000m, Dimension.Area, "km2", "square kilometer", "square kilometers");
            yield return MakeUnit(0.00064516m, Dimension.Area, "in2", "square inch", "square inches");
            yield return MakeUnit(0.092903m, Dimension.Area, "ft2", "square foot", "square feet");
            yield return MakeUnit(0.836127m, Dimension.Area, "yd2", "square yard", "square yards");
            yield return MakeUnit(4046.86m, Dimension.Area, "ac", "acre", "acres");
            yield return MakeUnit(258998m, Dimension.Area, "mi2", "square mile", "square miles");
        }
    }
}
