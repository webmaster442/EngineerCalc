using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.ConversionEngine;

internal abstract class UnitBase
{
    public required HashSet<string> Names { get; init; }
    public required Dimension Dimension { get; init; }

    public abstract decimal ToBaseUnit(decimal value);

    public abstract decimal FromBaseUnit(decimal value);
}
