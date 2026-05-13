using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.ConversionEngine;

internal sealed class RatioUnit : UnitBase
{
    public required ConversionNumber RatioToBaseUnit { get; init; }

    public override ConversionNumber FromBaseUnit(ConversionNumber value)
        => value / RatioToBaseUnit;

    public override ConversionNumber ToBaseUnit(ConversionNumber value)
        => value * RatioToBaseUnit;
}
