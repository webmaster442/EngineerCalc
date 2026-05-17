//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
