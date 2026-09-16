using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicEvaluator.TypeSystem.Internals;

internal static class InternalCasts
{
    public static int CastToInt(this Result result)
    {
        if (result.TypeState == TypeState.Integer)
        {
            var value = result.CastToBigInteger();
            return value >= int.MinValue && value <= int.MaxValue
                ? (int)value
                : throw new OverflowException($"Value {value} is out of range for Int32.");
        }
        throw new InvalidCastException($"Cannot cast value of type {result.TypeState} to Int32.");
    }
}
