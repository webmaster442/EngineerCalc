using System.Numerics;

using DynamicEvaluator.TypeSystem.Internals;
using DynamicEvaluator.TypeSystem.InternalTypes;

namespace DynamicEvaluator.TypeSystem;

public static class TypeFunctions
{
    #region General Functions
    public static Result Abs(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Double => Result.FromDouble(Math.Abs(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Abs(value.CastToComplex())),
            TypeState.Integer => Result.FromBigInteger(BigInteger.Abs(value.CastToBigInteger())),
            TypeState.Fraction => Result.FromFraction(Fraction.Abs(value.CastToFraction())),
            _ => throw TypeException.IncompatibleFunction(nameof(Abs), value.TypeState)
        };
    }

    public static Result Ln(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Log(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Log(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Log(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Ln), value.TypeState)
        };
    }

    public static Result Log(Result value, Result baseValue)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Log(value.CastToDouble(), baseValue.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Log(value.CastToDouble(), baseValue.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Log(value.CastToComplex(), baseValue.CastToDouble())),
            _ => throw TypeException.IncompatibleFunction(nameof(Log), value.TypeState, baseValue.TypeState)
        };
    }

    public static Result Pow(Result value, Result exponent)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Pow(value.CastToDouble(), exponent.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Pow(value.CastToDouble(), exponent.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Pow(value.CastToComplex(), exponent.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Pow), value.TypeState, exponent.TypeState)
        };
    }

    public static Result Root(Result value, Result degree)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Pow(value.CastToDouble(), 1.0 / degree.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Pow(value.CastToDouble(), 1.0 / degree.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Pow(value.CastToComplex(), 1.0 / degree.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Root), value.TypeState, degree.TypeState)
        };
    }

    public static Result Sqrt(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Sqrt(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Sqrt(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Sqrt(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Sqrt), value.TypeState)
        };
    }

    public static Result Gcd(Result a, Result b)
    {
        return (a.TypeState, b.TypeState) switch
        {
            (TypeState.Integer, TypeState.Integer) => Result.FromBigInteger(BigInteger.GreatestCommonDivisor(a.CastToBigInteger(), b.CastToBigInteger())),
            _ => throw TypeException.IncompatibleFunction(nameof(Gcd), a.TypeState, b.TypeState)
        };
    }

    public static Result Lcm(Result a, Result b)
    {
        return (a.TypeState, b.TypeState) switch
        {
            (TypeState.Integer, TypeState.Integer) => Result.FromBigInteger(IntegerMath.Lcm(a.CastToBigInteger(), b.CastToBigInteger())),
            _ => throw TypeException.IncompatibleFunction(nameof(Lcm), a.TypeState, b.TypeState)
        };
    }

    #endregion

    #region Trigonometric Functions
    public static Result Sin(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Sin(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Sin(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Sin(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Sin), value.TypeState)
        };
    }

    public static Result ArcSin(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Asin(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Asin(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Asin(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcSin), value.TypeState)
        };
    }

    public static Result Cos(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Cos(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Cos(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Cos(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Cos), value.TypeState)
        };
    }

    public static Result ArcCos(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Cos(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Acos(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Acos(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcCos), value.TypeState)
        };
    }

    public static Result Tan(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Tan(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Tan(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Tan(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Tan), value.TypeState)
        };
    }

    public static Result ArcTan(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Atan(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Atan(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Atan(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcTan), value.TypeState)
        };
    }
    #endregion

    #region Type Functions

    public static Result FromHex(Result value)
    {
        return value.TypeState switch
        {
            TypeState.String => Result.FromBigInteger(BigInteger.Parse(value.CastToString(), System.Globalization.NumberStyles.HexNumber)),
            _ => throw TypeException.IncompatibleFunction(nameof(FromHex), value.TypeState)
        };
    }

    public static Result FromBin(Result value)
    {
        return value.TypeState switch
        {
            TypeState.String => Result.FromBigInteger(BigInteger.Parse(value.CastToString(), System.Globalization.NumberStyles.BinaryNumber)),
            _ => throw TypeException.IncompatibleFunction(nameof(FromBin), value.TypeState)
        };
    }

    public static Result ToHex(Result value)
    {
        if (value.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(ToHex), value.TypeState);

        BigInteger casted = value.CastToBigInteger();
        if (casted < long.MaxValue)
        {
            return Result.FromString(((long)casted).ToString("X"));
        }
        return Result.FromString(casted.ToString("X"));
    }

    public static Result ToBin(Result value)
    {
        if (value.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(ToHex), value.TypeState);

        BigInteger casted = value.CastToBigInteger();
        if (casted < long.MaxValue)
        {
            return Result.FromString(((long)casted).ToString("B"));
        }
        return Result.FromString(casted.ToString("B"));
    }

    public static Result Cplx(Result real, Result imaginary)
    {
        return (real.TypeState, imaginary.TypeState) switch
        {
            (TypeState.Double, TypeState.Integer) => Result.FromComplex(new Complex(real.CastToDouble(), imaginary.CastToDouble())),
            (TypeState.Integer, TypeState.Double) => Result.FromComplex(new Complex(real.CastToDouble(), imaginary.CastToDouble())),
            (TypeState.Double, TypeState.Double) => Result.FromComplex(new Complex(real.CastToDouble(), imaginary.CastToDouble())),
            (TypeState.Integer, TypeState.Integer) => Result.FromComplex(new Complex((double)real.CastToBigInteger(), (double)imaginary.CastToBigInteger())),
            _ => throw TypeException.IncompatibleFunction(nameof(Cplx), real.TypeState, imaginary.TypeState)
        };
    }

    public static Result CplxPlr(Result magnitude, Result phase)
    {
        return (magnitude.TypeState, phase.TypeState) switch
        {
            (TypeState.Integer, TypeState.Double) => Result.FromComplex(Complex.FromPolarCoordinates(magnitude.CastToDouble(), phase.CastToDouble())),
            (TypeState.Double, TypeState.Integer) => Result.FromComplex(Complex.FromPolarCoordinates(magnitude.CastToDouble(), phase.CastToDouble())),
            (TypeState.Double, TypeState.Double) => Result.FromComplex(Complex.FromPolarCoordinates(magnitude.CastToDouble(), phase.CastToDouble())),
            (TypeState.Integer, TypeState.Integer) => Result.FromComplex(Complex.FromPolarCoordinates((double)magnitude.CastToBigInteger(), (double)phase.CastToBigInteger())),
            _ => throw TypeException.IncompatibleFunction(nameof(CplxPlr), magnitude.TypeState, phase.TypeState)
        };
    }

    #endregion

    #region Statistics Functions

    public static Result Max(params Result[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("At least one value is required.", nameof(values));

        if (values[0].TypeState == TypeState.Array)
            return Result.FromDouble(values[0].CastToArray().Max());

        Result max = values[0];

        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].TypeState != max.TypeState)
                throw TypeException.IncompatibleFunction(nameof(Max), max.TypeState, values[i].TypeState);

            if (values[i].CompareTo(max) > 0)
                max = values[i];
        }
        return max;
    }

    public static Result Min(params Result[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("At least one value is required.", nameof(values));

        if (values[0].TypeState == TypeState.Array)
            return Result.FromDouble(values[0].CastToArray().Min());

        Result min = values[0];
        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].TypeState != min.TypeState)
                throw TypeException.IncompatibleFunction(nameof(Min), min.TypeState, values[i].TypeState);

            if (values[i].CompareTo(min) < 0)
                min = values[i];
        }
        return min;
    }

    public static Result Sum(params Result[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("At least one value is required.", nameof(values));

        if (values[0].TypeState == TypeState.Array)
            return Result.FromDouble(values[0].CastToArray().Sum());

        Result sum = values[0];

        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].TypeState != sum.TypeState)
                throw TypeException.IncompatibleFunction(nameof(Sum), sum.TypeState, values[i].TypeState);

            sum += values[i];
        }
        return sum;
    }

    public static Result Average(params Result[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("At least one value is required.", nameof(values));
        if (values[0].TypeState == TypeState.Array)
            return Result.FromDouble(values[0].CastToArray().Average());

        Result sum = Sum(values);

        return sum / Result.FromBigInteger(values.Length);
    }

    #endregion
}
