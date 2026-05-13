using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.ConversionEngine;

internal abstract class UnitBase
{
    public required HashSet<string> Names { get; init; }
    public required Dimension Dimension { get; init; }

    public abstract ConversionNumber ToBaseUnit(ConversionNumber value);

    public abstract ConversionNumber FromBaseUnit(ConversionNumber value);
}
