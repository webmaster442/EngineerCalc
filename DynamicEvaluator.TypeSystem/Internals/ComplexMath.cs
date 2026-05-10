//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Numerics;

namespace DynamicEvaluator.TypeSystem.Internals;

internal static class ComplexMath
{
    public static Complex Asinh(Complex complex)
        => Complex.Log(complex + Complex.Sqrt((complex * complex) + 1));

    public static Complex Acosh(Complex complex)
        => Complex.Log(complex + Complex.Sqrt((complex * complex) - 1));

    public static Complex Atanh(Complex complex)
        => Complex.Log((1 + complex) / (1 - complex)) / 2;
}
