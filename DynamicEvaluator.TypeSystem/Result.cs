using System.Collections;
using System.Globalization;
using System.Numerics;

using DynamicEvaluator.TypeSystem.Internals;
using DynamicEvaluator.TypeSystem.InternalTypes;

namespace DynamicEvaluator.TypeSystem;

public sealed class Result :
    IEquatable<Result>,
    IComparable<Result>,
    IFormattable
{
    private readonly object _value;
    public TypeState TypeState { get; }

    private Result(object value, TypeState typeState)
    {
        _value = value;
        TypeState = typeState;
    }

    public static Result FromDouble(double value)
        => TypeHelper.CanBeInteger(value) ? new Result(new BigInteger(value), TypeState.Integer) : new Result(value, TypeState.Double);

    public double CastToDouble()
    {
        return TypeState switch
        {
            TypeState.Integer => (double)(BigInteger)_value,
            TypeState.Double => (double)_value,
            TypeState.Fraction => (double)(Fraction)_value,
            _ => throw new InvalidCastException($"Cannot cast type {TypeState} to double.")
        };
    }


    public static Result FromNumbers(params double[] values)
        => new Result(new DoubleArray(values), TypeState.Array);

    public IEnumerable<double> CastToArray()
    {
        return TypeState switch
        {
            TypeState.Array => (DoubleArray)_value,
            _ => throw new InvalidCastException($"Cannot cast type {TypeState} to array.")
        };
    }

    public static Result NoResult()
        => new Result(new NoResult(), TypeState.NoResult);

    public static Result FromComplex(Complex value)
        => TypeHelper.CanBeInteger(value) ? new Result(new BigInteger(value.Real), TypeState.Integer) : new Result(value, TypeState.Complex);

    public Complex CastToComplex()
    {
        return TypeState switch
        {
            TypeState.Integer => new Complex((double)(BigInteger)_value, 0),
            TypeState.Double => new Complex((double)_value, 0),
            TypeState.Fraction => new Complex((double)(Fraction)_value, 0),
            TypeState.Complex => (Complex)_value,
            _ => throw new InvalidCastException($"Cannot cast type {TypeState} to Complex.")
        };
    }

    public static Result FromBigInteger(BigInteger value)
        => new Result(value, TypeState.Integer);

    public BigInteger CastToBigInteger()
    {
        return TypeState switch
        {
            TypeState.Integer => (BigInteger)_value,
            TypeState.Double => new BigInteger((double)_value),
            TypeState.Fraction => new BigInteger((Fraction)_value),
            _ => throw new InvalidCastException($"Cannot cast type {TypeState} to BigInteger.")
        };
    }

    public static Result FromBoolean(bool value)
        => new Result(value, TypeState.Boolean);

    public bool CastToBoolean()
    {
        return TypeState switch
        {
            TypeState.Boolean => (bool)_value,
            _ => throw new InvalidCastException($"Cannot cast type {TypeState} to boolean.")
        };
    }

    public static Result FromString(string value)
        => new Result(value, TypeState.String);

    public string CastToString()
    {
        return TypeState switch
        {
            TypeState.String => (string)_value,
            _ => throw new InvalidCastException($"Cannot cast type {TypeState} to string.")
        };
    }

    public static Result FromFraction(Fraction value)
        => TypeHelper.CanBeInteger(value) ? new Result(value.Numerator, TypeState.Integer) : new Result(value, TypeState.Fraction);

    public Fraction CastToFraction()
    {
        return TypeState switch
        {
            TypeState.Integer => new Fraction((BigInteger)_value, 1),
            TypeState.Fraction => (Fraction)_value,
            _ => throw new InvalidCastException($"Cannot cast type {TypeState} to Fraction.")
        };
    }

    public bool Equals(Result? other)
    {
        return other is not null
            && TypeState == other.TypeState
            && _value.Equals(other._value);
    }

    public override bool Equals(object? obj)
        => Equals(obj as Result);

    public override int GetHashCode()
        => HashCode.Combine(TypeState, _value);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (_value is IFormattable formattable)
            return formattable.ToString(format, formatProvider);

        return _value.ToString() ?? string.Empty;
    }

    public string ToString(CultureInfo culture)
        => ToString(null, culture);

    public override string ToString()
        => ToString(CultureInfo.InvariantCulture);

    public int CompareTo(Result? other)
    {
        if (other is null)
            return 1;

        if (other._value is IComparable otherComparable
            && _value is IComparable comparable)
        {
            return comparable.CompareTo(otherComparable);
        }

        throw TypeException.Incompatible(TypeState, other.TypeState);
    }
}
