//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.ConversionEngine;

namespace DynamicEvaluator;

public sealed class UnitConverter
{
    private readonly UnitBase[] _units;

    public UnitConverter()
    {
        _units = UnitRegistry.Units.ToArray();
    }

    public double Convert(string fromUnit, string toUnit, double value)
    {
        UnitBase? from = _units
            .FirstOrDefault(unit => unit.Names.Contains(fromUnit));

        if (from is null)
            throw new ArgumentException($"Unknown unit: {fromUnit}");

        UnitBase? to = _units
            .FirstOrDefault(unit => unit.Names.Contains(toUnit));

        if (to is null)
            throw new ArgumentException($"Unknown unit: {toUnit}");

        if (from.Dimension != to.Dimension)
            throw new InvalidOperationException($"Cannot convert from {fromUnit} to {toUnit} because they have different dimensions.");

        decimal baseValue = from.ToBaseUnit((decimal)value);

        return (double)to.FromBaseUnit(baseValue);
    }
}
