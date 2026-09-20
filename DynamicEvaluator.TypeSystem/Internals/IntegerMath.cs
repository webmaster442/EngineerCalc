//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

    private static readonly (int Value, string Symbol)[] RomanNumerals =
    [
        (1000, "M"), 
        (900, "CM"),
        (500, "D"), 
        (400, "CD"),
        (100, "C"), 
        (90, "XC"), 
        (50, "L"),
        (40, "XL"),
        (10, "X"), 
        (9, "IX"),
        (5, "V"),
        (4, "IV"),
        (1, "I"),
    ];

    public static string ToRoman(BigInteger inputValue)
    {
        if (inputValue < 1 || inputValue > 3999)
        {
            throw new ArgumentOutOfRangeException(nameof(inputValue), "Roman numerals are only defined for values between 1 and 3999.");
        }

        int value = (int)inputValue;
        var builder = new System.Text.StringBuilder();
        foreach (var (numeralValue, symbol) in RomanNumerals)
        {
            while (value >= numeralValue)
            {
                builder.Append(symbol);
                value -= numeralValue;
            }
        }
        return builder.ToString();
    }

    public static BigInteger FromRoman(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Input must be a non-empty Roman numeral string.", nameof(value));
        }

        string input = value.Trim().ToUpperInvariant();
        int result = 0;
        int index = 0;
        foreach (var (numeralValue, symbol) in RomanNumerals)
        {
            while (index + symbol.Length <= input.Length
                && input.AsSpan(index, symbol.Length).SequenceEqual(symbol))
            {
                result += numeralValue;
                index += symbol.Length;
            }
        }

        return index != input.Length
            || result < 1
            || result > 3999
            || ToRoman(result) != input
            ? throw new FormatException($"'{value}' is not a valid Roman numeral.")
            : (BigInteger)result;
    }
}
