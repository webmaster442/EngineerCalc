//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Numerics;

namespace DynamicEvaluator.TypeSystem.Internals;

internal static class IntegerMath
{
    public static BigInteger Lcm(BigInteger a, BigInteger b)
    {
        checked
        {
            return (a * b) / BigInteger.GreatestCommonDivisor(a, b);
        }
    }

    internal static BigInteger Factorial(BigInteger limit)
    {
        if (limit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "Factorial is not defined for negative numbers.");
        }

        BigInteger result = 1;
        BigInteger i = 1;
        while (i <= limit)
        {
            result *= i;
            i++;
        }
        return result;
    }
}
