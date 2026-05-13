using DynamicEvaluator.ConversionEngine;
using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator;

internal static class UnitConverter
{
    public static double Convert(string fromUnit, string toUnit, double value)
    {
        UnitBase? from = UnitRegistry
            .GetAllUnits()
            .SelectMany(u => u)
            .FirstOrDefault(unit => unit.Names.Contains(fromUnit));

        if (from is null)
            throw new ArgumentException($"Unknown unit: {fromUnit}");

        UnitBase? to = UnitRegistry
            .GetAllUnits()
            .SelectMany(u => u)
            .FirstOrDefault(unit => unit.Names.Contains(toUnit));

        if (to is null)
            throw new ArgumentException($"Unknown unit: {toUnit}");

        if (from.Dimension != to.Dimension)
            throw new InvalidOperationException($"Cannot convert from {fromUnit} to {toUnit} because they have different dimensions.");

        ConversionNumber baseValue = from.ToBaseUnit(value);
        
        return to.FromBaseUnit(baseValue).ToDouble();
    }
}
