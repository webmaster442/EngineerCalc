//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

using DynamicEvaluator.Expressions;
using DynamicEvaluator.TypeSystem;
using DynamicEvaluator.TypeSystem.InternalTypes;

namespace DynamicEvaluator;

public static partial class Extensions
{
    [GeneratedRegex(@"[\""].+?[\""]|\S+")]
    private static partial Regex QuotesMatcher();

    public static IEnumerable<string> SplitBySpaceOrQuotes(this string input)
    {
        var matches = QuotesMatcher().Matches(input);
        foreach (Match match in matches)
        {
            string value = match.Value;
            if ((value.StartsWith('"') && value.EndsWith('"')) ||
                (value.StartsWith('\'') && value.EndsWith('\'')))
            {
                yield return value.Substring(1, value.Length - 2);
            }
            else
            {
                yield return value;
            }
        }
    }

    internal static Result CreateResult(this Token token, CultureInfo culture)
    {
        if (!token.TypeState.HasValue)
            throw new InvalidOperationException($"Can't create a Type from: {token.Value}");

        return token.TypeState switch
        {
            TypeState.NoResult => Result.NoResult(),
            TypeState.Boolean => Result.FromBoolean(bool.Parse(token.Value)),
            TypeState.Integer => Result.FromBigInteger(BigInteger.Parse(token.Value, culture)),
            TypeState.Double => Result.FromDouble(double.Parse(token.Value, culture)),
            TypeState.Fraction => Result.FromFraction(Fraction.Parse(token.Value, culture)),
            TypeState.Complex => Result.FromComplex(Complex.Parse(token.Value, culture)),
            TypeState.Array => throw new InvalidOperationException("Array parsing is not supported"),
            TypeState.String => Result.FromString(token.Value),
            _ => throw new UnreachableException($"Unknown token type state: {token.TypeState}"),
        };
    }
}
