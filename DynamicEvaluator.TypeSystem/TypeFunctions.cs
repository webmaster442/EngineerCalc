//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
            TypeState.Fraction => Result.FromDouble(Math.Log(value.CastToDouble())),
            _ => throw TypeException.IncompatibleFunction(nameof(Ln), value.TypeState)
        };
    }

    public static Result Log(Result value, Result baseValue)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Log(value.CastToDouble(), baseValue.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Log(value.CastToDouble(), baseValue.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Log(value.CastToDouble(), baseValue.CastToDouble())),
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
            TypeState.Fraction => Result.FromDouble(Math.Pow(value.CastToDouble(), exponent.CastToDouble())),
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
            TypeState.Fraction => Result.FromDouble(Math.Pow(value.CastToDouble(), 1.0 / degree.CastToDouble())),
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
            TypeState.Fraction => Result.FromDouble(Math.Sqrt(value.CastToDouble())),
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

    public static Result Random(params Result[] args)
    {
        if (args.Length == 0)
            return Result.FromBigInteger(System.Random.Shared.NextInt64());

        if (args.Length == 1)
            return Result.FromBigInteger(System.Random.Shared.NextInt64((long)args[0].CastToBigInteger()));

        if (args.Length == 2)
            return Result.FromBigInteger(System.Random.Shared.NextInt64((long)args[0].CastToBigInteger(), (long)args[1].CastToBigInteger()));

        throw new InvalidOperationException($"Too many arguments for: {nameof(Random)}");
    }

    public static Result Factorial(Result value)
    {
        return Result.FromBigInteger(IntegerMath.Factorial(value.CastToBigInteger()));
    }

    public static Result Not(Result value)
    {
        if (value.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(Not), value.TypeState);

        return Result.FromBigInteger(~value.CastToBigInteger());
    }

    public static Result And(Result a, Result b)
    {
        if (a.TypeState != TypeState.Integer || b.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(And), a.TypeState, b.TypeState);

        return Result.FromBigInteger(a.CastToBigInteger() & b.CastToBigInteger());
    }

    public static Result Or(Result a, Result b)
    {
        if (a.TypeState != TypeState.Integer || b.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(Or), a.TypeState, b.TypeState);

        return Result.FromBigInteger(a.CastToBigInteger() | b.CastToBigInteger());
    }

    public static Result Xor(Result a, Result b)
    {
        if (a.TypeState != TypeState.Integer || b.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(Xor), a.TypeState, b.TypeState);

        return Result.FromBigInteger(a.CastToBigInteger() ^ b.CastToBigInteger());
    }

    public static Result ShiftLeft(Result a, Result b)
    {
        if (a.TypeState != TypeState.Integer || b.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(ShiftLeft), a.TypeState, b.TypeState);

        BigInteger shiftAmmount = b.CastToBigInteger();

        if (shiftAmmount < 0)
            throw new ArgumentOutOfRangeException(nameof(b), "Shift amount cannot be negative.");

        if (shiftAmmount > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(b), $"Shift amount cannot be greater than {int.MaxValue}.");

        return Result.FromBigInteger(a.CastToBigInteger() << (int)shiftAmmount);
    }

    public static Result ShiftRight(Result a, Result b)
    {
        if (a.TypeState != TypeState.Integer || b.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(ShiftRight), a.TypeState, b.TypeState);

        BigInteger shiftAmmount = b.CastToBigInteger();

        if (shiftAmmount < 0)
            throw new ArgumentOutOfRangeException(nameof(b), "Shift amount cannot be negative.");

        if (shiftAmmount > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(b), $"Shift amount cannot be greater than {int.MaxValue}.");

        return Result.FromBigInteger(a.CastToBigInteger() >> (int)shiftAmmount);
    }

    public static Result Binomial(Result n, Result k)
    {
        if (n.TypeState != TypeState.Integer || k.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(Binomial), n.TypeState, k.TypeState);

        var nValue = n.CastToBigInteger();
        var kValue = k.CastToBigInteger();

        if (nValue > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(n), $"n cannot be greater than {int.MaxValue}.");

        if (kValue > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(k), $"k cannot be greater than {int.MaxValue}.");

        return Result.FromBigInteger(IntegerMath.Binomial((int)nValue, (int)kValue));
    }

    public static Result Floor(Result value)
        => Result.FromDouble(Math.Floor(value.CastToDouble()));

    public static Result Ceiling(Result value)
        => Result.FromDouble(Math.Ceiling(value.CastToDouble()));

    #endregion

    #region Trigonometric Functions
    public static Result Sin(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Sin(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Sin(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Sin(value.CastToDouble())),
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
            TypeState.Fraction => Result.FromDouble(Math.Asin(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Asin(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcSin), value.TypeState)
        };
    }


    public static Result Sinh(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Sinh(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Sinh(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Sinh(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Sinh(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Sinh), value.TypeState)
        };
    }

    public static Result ArcSinh(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Asinh(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Asinh(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Asinh(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(ComplexMath.Asinh(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcSinh), value.TypeState)
        };
    }

    public static Result Cos(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Cos(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Cos(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Cos(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Cos(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Cos), value.TypeState)
        };
    }

    public static Result ArcCos(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Acos(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Acos(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Acos(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Acos(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcCos), value.TypeState)
        };
    }

    public static Result Cosh(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Cosh(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Cosh(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Cosh(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Cosh(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Cosh), value.TypeState)
        };
    }

    public static Result ArcCosh(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Acosh(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Acosh(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Acosh(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(ComplexMath.Acosh(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcCosh), value.TypeState)
        };
    }

    public static Result Tan(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Tan(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Tan(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Tan(value.CastToDouble())),
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
            TypeState.Fraction => Result.FromDouble(Math.Atan(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Atan(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcTan), value.TypeState)
        };
    }

    public static Result Tanh(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Tanh(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Tanh(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Tanh(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(Complex.Tanh(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(Tanh), value.TypeState)
        };
    }


    public static Result ArcTanh(Result value)
    {
        return value.TypeState switch
        {
            TypeState.Integer => Result.FromDouble(Math.Atanh(value.CastToDouble())),
            TypeState.Double => Result.FromDouble(Math.Atanh(value.CastToDouble())),
            TypeState.Fraction => Result.FromDouble(Math.Atanh(value.CastToDouble())),
            TypeState.Complex => Result.FromComplex(ComplexMath.Atanh(value.CastToComplex())),
            _ => throw TypeException.IncompatibleFunction(nameof(ArcTanh), value.TypeState)
        };
    }

    #endregion

    #region Type Functions

    public static Result FromHex(Result value)
    {
        if (value.TypeState != TypeState.String)
            throw TypeException.IncompatibleFunction(nameof(FromHex), value.TypeState);

        return Result.FromBigInteger(BigInteger.Parse($"0{value.CastToString()}", System.Globalization.NumberStyles.HexNumber));
    }

    public static Result FromHexSigned(Result value)
    {
        if (value.TypeState != TypeState.String)
            throw TypeException.IncompatibleFunction(nameof(FromHexSigned), value.TypeState);

        return Result.FromBigInteger(BigInteger.Parse(value.CastToString(), System.Globalization.NumberStyles.HexNumber));
    }

    public static Result FromBin(Result value)
    {
        if (value.TypeState != TypeState.String)
            throw TypeException.IncompatibleFunction(nameof(FromBin), value.TypeState);

        return Result.FromBigInteger(BigInteger.Parse($"0{value.CastToString()}", System.Globalization.NumberStyles.BinaryNumber));
    }

    public static Result FromBinSigned(Result value)
    {
        if (value.TypeState != TypeState.String)
            throw TypeException.IncompatibleFunction(nameof(FromBinSigned), value.TypeState);

        return Result.FromBigInteger(BigInteger.Parse(value.CastToString(), System.Globalization.NumberStyles.BinaryNumber));
    }

    public static Result ToHex(Result value)
    {
        if (value.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(ToHex), value.TypeState);

        BigInteger casted = value.CastToBigInteger();
        string hex = casted.ToString("X");

        if (casted > BigInteger.Zero)
        {
            string trimmed = hex.TrimStart('0');
            return Result.FromString(trimmed.Length == 0 ? "0" : trimmed);
        }

        return Result.FromString(hex);
    }

    public static Result ToBin(Result value)
    {
        if (value.TypeState != TypeState.Integer)
            throw TypeException.IncompatibleFunction(nameof(ToHex), value.TypeState);

        BigInteger casted = value.CastToBigInteger();

        if (casted > BigInteger.Zero)
        {
            //remove leading zeros
            return Result.FromString(casted.ToString("B")[1..]);
        }

        return Result.FromString(casted.ToString("B"));
    }

    public static Result Cplx(Result real, Result imaginary)
    {
        return Result.FromComplex(new Complex(real.CastToDouble(), imaginary.CastToDouble()));
    }

    public static Result CplxPlr(Result magnitude, Result phase)
    {
        return Result.FromComplex(Complex.FromPolarCoordinates(magnitude.CastToDouble(), phase.CastToDouble()));
    }

    public static Result Array(params Result[] values)
    {
        List<double> doubles = new List<double>();
        foreach (var value in values)
        {
            if (value.TypeState != TypeState.Integer && value.TypeState != TypeState.Double)
                throw TypeException.IncompatibleFunction(nameof(Array), value.TypeState);

            doubles.Add(value.CastToDouble());
        }
        return Result.FromNumbers(doubles);
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

    public static Result Count(params Result[] values)
    {
        if (values.Length == 0)
            throw new ArgumentException("At least one value is required.", nameof(values));

        if (values[0].TypeState == TypeState.Array)
            return Result.FromDouble(values[0].CastToArray().Count);

        return Result.FromBigInteger(values.Length);
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

        if (sum.TypeState == TypeState.Integer)
            return Result.FromFraction(new Fraction(sum.CastToBigInteger(), values.Length));


        return sum / Result.FromBigInteger(values.Length);
    }

    #endregion
}
