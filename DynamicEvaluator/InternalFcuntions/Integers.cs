//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.InternalFcuntions;

internal static class Integers
{
    public static long Lcm(long a, long b)
    {
        checked
        {
            return (a * b) / GreatestCommonDivisor(a, b);
        }
    }

    public static long GreatestCommonDivisor(long a, long b)
    {
        checked
        {
            if (a == 0 || b == 0)
            {
                return Math.Abs(a) + Math.Abs(b);
            }
            a = Math.Abs(a);
            b = Math.Abs(b);
            int shift = 0;
            while (((a | b) & 1) == 0)
            {
                a >>= 1;
                b >>= 1;
                shift++;
            }
            while ((a & 1) == 0)
            {
                a >>= 1;
            }
            do
            {
                while ((b & 1) == 0)
                {
                    b >>= 1;
                }
                if (a > b)
                {
                    (b, a) = (a, b);
                }
                b -= a;
            } while (b != 0);

            return a << shift;
        }
    }

    public static int Bits(long number)
    {
        int bits = 1;
        if (number < 0)
        {
            number = -number;
            ++bits; // sign bit
        }

        while (number > 1)
        {
            bits++;
            number >>= 1;
        }
        return bits;
    }

    public static IEnumerable<long> GetPrimeFactors(long number)
    {
        if (number < 0)
        {
            number = -number;
            yield return -1;
        }

        if (number < 2)
            yield break;

        while ((number & 1) == 0)
        {
            yield return 2;
            number >>= 1; // equivalent to n /= 2, but faster
        }

        for (long i = 3; i * i <= number; i += 2)
        {
            while (number % i == 0)
            {
                yield return i;
                number /= i;
            }
        }

        if (number > 1)
            yield return number;
    }
}
