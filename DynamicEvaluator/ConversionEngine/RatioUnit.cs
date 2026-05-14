using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.ConversionEngine;

internal sealed class RatioUnit : UnitBase
{
    public required decimal RatioToBaseUnit { get; init; }

    public override decimal FromBaseUnit(decimal value)
        => value / RatioToBaseUnit;

    public override decimal ToBaseUnit(decimal value)
        => value * RatioToBaseUnit;
}
