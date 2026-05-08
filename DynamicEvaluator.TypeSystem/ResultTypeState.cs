//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.TypeSystem;

public enum ResultTypeState
{
    Incompatible,
    NoResult,
    Boolean,
    Integer,
    Double,
    Fraction,
    Complex,
    Array,
    String
}
