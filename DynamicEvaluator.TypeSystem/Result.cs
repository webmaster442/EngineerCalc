using System.Globalization;
using System.Numerics;

using DynamicEvaluator.TypeSystem.Internals;
using DynamicEvaluator.TypeSystem.InternalTypes;

namespace DynamicEvaluator.TypeSystem;

public sealed class Result :
    IComparable<Result>,
    IEquatable<Result>,
    IFormattable,
    IAdditionOperators<Result, Result, Result>,
    ISubtractionOperators<Result, Result, Result>,
    IMultiplyOperators<Result, Result, Result>,
    IDivisionOperators<Result, Result, Result>
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
        ResultTypeState resultType = TypeHelper.GetResultTypeState(TypeState, other?.TypeState ?? TypeState.NoResult);

        return resultType switch
        {
            ResultTypeState.Incompatible => false,
            ResultTypeState.NoResult => true,
            ResultTypeState.Integer => CastToBigInteger() == other!.CastToBigInteger(),
            ResultTypeState.Double => CastToDouble() == other!.CastToDouble(),
            ResultTypeState.Fraction => CastToFraction() == other!.CastToFraction(),
            ResultTypeState.Complex => CastToComplex() == other!.CastToComplex(),
            ResultTypeState.Array => CastToArray().SequenceEqual(other!.CastToArray()),
            ResultTypeState.String => CastToString() == other!.CastToString(),
            ResultTypeState.Boolean => CastToBoolean() == other!.CastToBoolean(),
            _ => false,
        };
    }

    public int CompareTo(Result? other)
    {
        if (other == null)
            return 1;

        ResultTypeState resultType = TypeHelper.GetResultTypeState(TypeState, other?.TypeState ?? TypeState.NoResult);

        return resultType switch
        {
            ResultTypeState.Incompatible => -1,
            ResultTypeState.NoResult => 0,
            ResultTypeState.Integer => CastToBigInteger().CompareTo(other.CastToBigInteger()),
            ResultTypeState.Double => CastToDouble().CompareTo(other.CastToDouble()),
            ResultTypeState.Fraction => CastToFraction().CompareTo(other.CastToFraction()),
            ResultTypeState.Complex => throw new InvalidOperationException("Cannot compare complex numbers."),
            ResultTypeState.Array => throw new InvalidOperationException("Cannot compare arrays."),
            ResultTypeState.String => string.Compare(CastToString(), other.CastToString(), StringComparison.Ordinal),
            ResultTypeState.Boolean => CastToBoolean().CompareTo(other.CastToBoolean()),
            _ => throw new InvalidOperationException("Unknown result type state.")
        };
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

    public static bool operator <(Result left, Result right)
        => left.CompareTo(right) < 0;

    public static bool operator <=(Result left, Result right)
        => left.CompareTo(right) <= 0;

    public static bool operator >(Result left, Result right)
        => left.CompareTo(right) > 0;

    public static bool operator >=(Result left, Result right)
        => left.CompareTo(right) >= 0;

    public static bool operator ==(Result? left, Result? right)
        => EqualityComparer<Result>.Default.Equals(left, right);

    public static bool operator !=(Result? left, Result? right)
        => !(left == right);

    public static Result operator +(Result left, Result right)
    {
        ResultTypeState resultType = TypeHelper.GetResultTypeState(left.TypeState, right.TypeState);

        return resultType switch
        {
            ResultTypeState.Incompatible => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "+"),
            ResultTypeState.NoResult => NoResult(),
            ResultTypeState.Integer => FromBigInteger(left.CastToBigInteger() + right.CastToBigInteger()),
            ResultTypeState.Double => FromDouble(left.CastToDouble() + right.CastToDouble()),
            ResultTypeState.Fraction => FromFraction(left.CastToFraction() + right.CastToFraction()),
            ResultTypeState.Complex => FromComplex(left.CastToComplex() + right.CastToComplex()),
            ResultTypeState.Array => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "+"),
            ResultTypeState.String => FromString(left.CastToString() + right.CastToString()),
            ResultTypeState.Boolean => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "+"),
            _ => throw new InvalidOperationException("Unknown result type state."),
        };
    }

    public static Result operator -(Result left, Result right)
    {
        ResultTypeState resultType = TypeHelper.GetResultTypeState(left.TypeState, right.TypeState);

        return resultType switch
        {
            ResultTypeState.Incompatible => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "-"),
            ResultTypeState.NoResult => NoResult(),
            ResultTypeState.Integer => FromBigInteger(left.CastToBigInteger() - right.CastToBigInteger()),
            ResultTypeState.Double => FromDouble(left.CastToDouble() - right.CastToDouble()),
            ResultTypeState.Fraction => FromFraction(left.CastToFraction() - right.CastToFraction()),
            ResultTypeState.Complex => FromComplex(left.CastToComplex() - right.CastToComplex()),
            ResultTypeState.Array => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "-"),
            ResultTypeState.String => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "-"),
            ResultTypeState.Boolean => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "-"),
            _ => throw new InvalidOperationException("Unknown result type state."),
        };
    }

    public static Result operator * (Result left, Result right)
    {
        ResultTypeState resultType = TypeHelper.GetResultTypeState(left.TypeState, right.TypeState);

        return resultType switch
        {
            ResultTypeState.Incompatible => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "*"),
            ResultTypeState.NoResult => NoResult(),
            ResultTypeState.Integer => FromBigInteger(left.CastToBigInteger() * right.CastToBigInteger()),
            ResultTypeState.Double => FromDouble(left.CastToDouble() * right.CastToDouble()),
            ResultTypeState.Fraction => FromFraction(left.CastToFraction() * right.CastToFraction()),
            ResultTypeState.Complex => FromComplex(left.CastToComplex() * right.CastToComplex()),
            ResultTypeState.Array => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "*"),
            ResultTypeState.String => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "*"),
            ResultTypeState.Boolean => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "*"),
            _ => throw new InvalidOperationException("Unknown result type state."),
        };
    }

    public static Result operator /(Result left, Result right)
    {
        ResultTypeState resultType = TypeHelper.GetResultTypeState(left.TypeState, right.TypeState);

        return resultType switch
        {
            ResultTypeState.Incompatible => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "/"),
            ResultTypeState.NoResult => NoResult(),
            ResultTypeState.Integer => FromBigInteger(left.CastToBigInteger() / right.CastToBigInteger()),
            ResultTypeState.Double => FromDouble(left.CastToDouble() / right.CastToDouble()),
            ResultTypeState.Fraction => FromFraction(left.CastToFraction() / right.CastToFraction()),
            ResultTypeState.Complex => FromComplex(left.CastToComplex() / right.CastToComplex()),
            ResultTypeState.Array => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "/"),
            ResultTypeState.String => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "/"),
            ResultTypeState.Boolean => throw TypeException.IncompatibleOperator(left.TypeState, right.TypeState, "/"),
            _ => throw new InvalidOperationException("Unknown result type state."),
        };
    }
}
