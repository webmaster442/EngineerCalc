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

    public static BigInteger Binomial(int n, int k)
    {
        if (k < 0 || k > n)
            return BigInteger.Zero;

        if (k == 0 || k == n)
            return BigInteger.One;

        // Symmetry optimization
        k = Math.Min(k, n - k);

        BigInteger result = BigInteger.One;

        for (int i = 1; i <= k; i++)
        {
            result *= n - (k - i);
            result /= i;
        }

        return result;
    }
}
