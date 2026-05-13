using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.ConversionEngine;

internal static class UnitRegistry
{
    private static RatioUnit MakeUnit(ConversionNumber ratio, Dimension dimension, params string[] names)
    {
        return new RatioUnit
        {
            Dimension = dimension,
            RatioToBaseUnit = ratio,
            Names = new HashSet<string>(names, StringComparer.InvariantCultureIgnoreCase)
        };
    }

    public static IEnumerable<RatioUnit> GetMassUnits()
    {
        yield return MakeUnit(1, Dimension.Mass, "g", "gram", "grams");
        yield return MakeUnit(1000, Dimension.Mass, "kg", "kilogram", "kilograms");
        yield return MakeUnit(new ConversionNumber(45359237, 100000), Dimension.Mass, "lb", "pound", "pounds");
        yield return MakeUnit(new ConversionNumber(283495231, 10000000), Dimension.Mass, "oz", "ounce", "ounces");
    }

    public static IEnumerable<IEnumerable<RatioUnit>> GetAllUnits()
    {
        yield return GetMassUnits();
    }
}
