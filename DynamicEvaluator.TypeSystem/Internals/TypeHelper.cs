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

    public static TypeState GetResultTypeState(TypeState left, TypeState right)
    {
        if (left == TypeState.String
            || left == TypeState.Array
            || right == TypeState.String
            || right == TypeState.Array)
        {
            throw TypeException.Incompatible(left, right);
        }

        if ((left == TypeState.NoResult && right != TypeState.NoResult)
            || (left != TypeState.NoResult && right == TypeState.NoResult))
        {
            throw TypeException.Incompatible(left, right);
        }

        if ((left == TypeState.Boolean && right != TypeState.Boolean)
            || (left != TypeState.Boolean && right == TypeState.Boolean))
        {
            throw TypeException.Incompatible(left, right);
        }

        if (left == right)
            return left;

        if (left == TypeState.Integer)
        {
            return right switch
            {
                TypeState.Double => TypeState.Double,
                TypeState.Fraction => TypeState.Fraction,
                TypeState.Complex => TypeState.Complex,
                _ => throw TypeException.ShouldNotHappen(left, right),
            };
        }

        if (left == TypeState.Double)
        {
            return right switch
            {
                TypeState.Integer => TypeState.Double,
                TypeState.Fraction => TypeState.Double,
                TypeState.Complex => TypeState.Complex,
                _ => throw TypeException.ShouldNotHappen(left, right),
            };
        }

        if (left == TypeState.Fraction)
        {
            return right switch
            {
                TypeState.Integer => TypeState.Fraction,
                TypeState.Double => TypeState.Double,
                TypeState.Complex => TypeState.Complex,
                _ => throw TypeException.ShouldNotHappen(left, right),
            };
        }

        if (left == TypeState.Complex)
        {
            return right switch
            {
                TypeState.Integer => TypeState.Complex,
                TypeState.Double => TypeState.Complex,
                TypeState.Fraction => TypeState.Complex,
                _ => throw TypeException.ShouldNotHappen(left, right),
            };
        }

        throw TypeException.ShouldNotHappen(left, right);
    }
}
