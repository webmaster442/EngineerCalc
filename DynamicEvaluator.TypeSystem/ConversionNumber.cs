using System.Numerics;

using DynamicEvaluator.TypeSystem.Internals;
using DynamicEvaluator.TypeSystem.InternalTypes;

namespace DynamicEvaluator.TypeSystem;

public readonly struct ConversionNumber :
    IAdditionOperators<ConversionNumber, ConversionNumber, ConversionNumber>,
    ISubtractionOperators<ConversionNumber, ConversionNumber, ConversionNumber>,
    IMultiplyOperators<ConversionNumber, ConversionNumber, ConversionNumber>,
    IDivisionOperators<ConversionNumber, ConversionNumber, ConversionNumber>
{
    private readonly double _doubleValue;
    private readonly Fraction _fraction;
    private readonly bool _isFraction;

    public ConversionNumber(double doubleValue)
    {
        if (TypeHelper.CanBeInteger(doubleValue))
        {
            _fraction = new Fraction((long)doubleValue, 1);
            _isFraction = true;
            _doubleValue = default;
        }
        else
        {
            _isFraction = false;
            _doubleValue = doubleValue;
            _fraction = default;
        }
    }

    public ConversionNumber(long numerator, long denominator = 1)
    {
        _isFraction = true;
        _fraction = new Fraction(numerator, denominator);
        _doubleValue = default;
    }

    public readonly double ToDouble()
        => _isFraction ? (double)_fraction : _doubleValue;

    public static ConversionNumber operator +(ConversionNumber left, ConversionNumber right)
    {
        if (left._isFraction && right._isFraction)
            return new ConversionNumber(left._fraction + right._fraction);

        return new ConversionNumber(left.ToDouble() + right.ToDouble());
    }

    public static ConversionNumber operator -(ConversionNumber left, ConversionNumber right)
    {
        if (left._isFraction && right._isFraction)
            return new ConversionNumber(left._fraction - right._fraction);

        return new ConversionNumber(left.ToDouble() - right.ToDouble());
    }

    public static ConversionNumber operator /(ConversionNumber left, ConversionNumber right)
    {
        if (left._isFraction && right._isFraction)
            return new ConversionNumber(left._fraction / right._fraction);

        return new ConversionNumber(left.ToDouble() / right.ToDouble());
    }

    public static ConversionNumber operator *(ConversionNumber left, ConversionNumber right)
    {
        if (left._isFraction && right._isFraction)
            return new ConversionNumber(left._fraction * right._fraction);

        return new ConversionNumber(left.ToDouble() * right.ToDouble());
    }

    public static implicit operator ConversionNumber(double value)
        => new ConversionNumber(value);

    public static implicit operator ConversionNumber(long value)
        => new ConversionNumber(value);
}
