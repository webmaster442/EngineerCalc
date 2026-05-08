//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Numerics;

using DynamicEvaluator.TypeSystem.InternalTypes;

namespace DynamicEvaluator.TypeSystem.Internals;

internal static class TypeHelper
{
    private const double MaxIntegerValue = 9007199254740991d;
    private const double MinIntegerValue = -9007199254740990d;

    public static bool CanBeInteger(double value)
    {
        return !double.IsNaN(value)
            && !double.IsInfinity(value)
            && value >= MinIntegerValue
            && value <= MaxIntegerValue
            && Math.Abs(value - Math.Truncate(value)) < double.Epsilon;
    }

    public static bool CanBeInteger(Fraction value)
        => value.Denominator == 1;

    public static bool CanBeInteger(Complex value)
    {
        return !double.IsNaN(value.Imaginary)
            && !double.IsInfinity(value.Imaginary)
            && Math.Abs(0 - value.Imaginary) < double.Epsilon
            && CanBeInteger(value.Real);
    }

    //              NoResult	    Boolean	        Integer	        Double	        Fraction	    Complex	        Array	        String
    //  NoResult	NoResult	    Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	Incompatible
    //  Boolean	    Incompatible	Boolean	        Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	Incompatible
    //  Ingeger	    Incompatible	Incompatible	Integer	        Double	        Fraction	    Complex	        Incompatible	Incompatible
    //  Double	    Incompatible	Incompatible	Double	        Double	        Double	        Complex	        Incompatible	Incompatible
    //  Fraction	Incompatible	Incompatible	Fraction	    Double	        Fraction	    Complex	        Incompatible	Incompatible
    //  Complex	    Incompatible	Incompatible	Complex	        Complex	        Complex	        Complex	        Incompatible	Incompatible
    //  Array	    Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	Array	        Incompatible
    //  String	    Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	Incompatible	String
    public static ResultTypeState GetResultTypeState(TypeState left, TypeState right)
    {
        if (left == right)
            return ToResultTypeState(left);

        return (left, right) switch
        {
            (TypeState.Integer, TypeState.Double) => ResultTypeState.Double,
            (TypeState.Integer, TypeState.Fraction) => ResultTypeState.Fraction,
            (TypeState.Integer, TypeState.Complex) => ResultTypeState.Complex,
            (TypeState.Double, TypeState.Integer) => ResultTypeState.Double,
            (TypeState.Double, TypeState.Fraction) => ResultTypeState.Double,
            (TypeState.Double, TypeState.Complex) => ResultTypeState.Complex,
            (TypeState.Fraction, TypeState.Integer) => ResultTypeState.Fraction,
            (TypeState.Fraction, TypeState.Double) => ResultTypeState.Double,
            (TypeState.Fraction, TypeState.Complex) => ResultTypeState.Complex,
            (TypeState.Complex, TypeState.Integer) => ResultTypeState.Complex,
            (TypeState.Complex, TypeState.Double) => ResultTypeState.Complex,
            (TypeState.Complex, TypeState.Fraction) => ResultTypeState.Complex,
            (_, _) => ResultTypeState.Incompatible,
        };
    }

    private static ResultTypeState ToResultTypeState(TypeState left)
    {
        return left switch
        {
            TypeState.NoResult => ResultTypeState.NoResult,
            TypeState.Boolean => ResultTypeState.Boolean,
            TypeState.Integer => ResultTypeState.Integer,
            TypeState.Double => ResultTypeState.Double,
            TypeState.Fraction => ResultTypeState.Fraction,
            TypeState.Complex => ResultTypeState.Complex,
            TypeState.Array => ResultTypeState.Array,
            TypeState.String => ResultTypeState.String,
            _ => throw new UnreachableException($"Unexpected TypeState: {left}"),
        };
    }
}
