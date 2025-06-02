using System.Numerics;

using DynamicEvaluator.InternalFcuntions;
using DynamicEvaluator.Types;

namespace DynamicEvaluator;

public static class Functions
{
    public static dynamic Abs(dynamic value)
    {
        if (value is Complex c)
            return Complex.Abs(c);

        if (value is Fraction fr)
            return Fraction.Abs(fr);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Abs(d);

        throw new InvalidOperationException($"Can't perform Abs function on type {value.GetType()}");
    }

    public static dynamic Ln(dynamic value)
    {
        if (value is Complex c)
            return Complex.Log(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Log(d);

        throw new InvalidOperationException($"Can't perform Ln function on type {value.GetType()}");
    }

    public static dynamic Sin(dynamic value)
    {
        if (value is Complex c)
            return Complex.Sin(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Sin(d);

        throw new InvalidOperationException($"Can't perform Sin function on type {value.GetType()}");
    }

    public static dynamic ArcSin(dynamic value)
    {
        if (value is Complex c)
            return Complex.Asin(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Asin(d);

        throw new InvalidOperationException($"Can't perform ArcSin function on type {value.GetType()}");
    }

    public static dynamic Cos(dynamic value)
    {
        if (value is Complex c)
            return Complex.Cos(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Cos(d);

        throw new InvalidOperationException($"Can't perform Cos function on type {value.GetType()}");
    }

    public static dynamic ArcCos(dynamic value)
    {
        if (value is Complex c)
            return Complex.Acos(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Acos(d);

        throw new InvalidOperationException($"Can't perform ArcCos function on type {value.GetType()}");
    }

    public static dynamic Tan(dynamic value)
    {
        if (value is Complex c)
            return Complex.Tan(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Tan(d);

        throw new InvalidOperationException($"Can't perform Tan function on type {value.GetType()}");
    }

    public static dynamic ArcTan(dynamic value)
    {
        if (value is Complex c)
            return Complex.Atan(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Atan(d);

        throw new InvalidOperationException($"Can't perform ArcTan function on type {value.GetType()}");
    }

    public static dynamic Log(dynamic number, dynamic @base)
    {
        if (TypeFactory.DynamicConvert<double>(number, out double d1)
            & TypeFactory.DynamicConvert<double>(@base, out double d2))
        {
            return Math.Log(d1, d2);
        }

        if (TypeFactory.DynamicConvert<Complex>(number, out Complex c1)
            & TypeFactory.DynamicConvert<double>(@base, out double d))
        {
            return Complex.Log(c1, d);
        }

        throw new InvalidOperationException($"Can't perform Log function on type {number.GetType()} and {@base.GetType()}");
    }

    public static dynamic Pow(dynamic x, dynamic y)
    {
        if (TypeFactory.DynamicConvert<double>(x, out double d1)
            & TypeFactory.DynamicConvert<double>(y, out double d2))
        {
            return Math.Pow(d1, d2);
        }

        if (TypeFactory.DynamicConvert<Complex>(x, out Complex c1)
            & TypeFactory.DynamicConvert<double>(y, out double d))
        {
            return Complex.Pow(c1, d);
        }

        throw new InvalidOperationException($"Can't perform Pow function on type {x.GetType()} and {y.GetType()}");
    }

    public static dynamic Root(dynamic x, dynamic y)
        => Pow(x, 1 / y);

    public static long Hex(string str)
        => long.Parse(str, System.Globalization.NumberStyles.HexNumber);

    public static string ToHex(long number)
        => Convert.ToString(number, 16).ToUpper();

    public static long Bin(string str)
        => long.Parse(str, System.Globalization.NumberStyles.BinaryNumber);

    public static string ToBin(long number)
        => Convert.ToString(number, 2);

    public static long Lcm(long a, long b)
        => Integers.Lcm(a, b);

    public static Complex Cplx(double x, double y)
        => new Complex(x, y);

    public static long Random()
        => System.Random.Shared.NextInt64();

    public static long Random(long minimum, long maximum)
        => System.Random.Shared.NextInt64(minimum, maximum);

    public static double Ceiling(double x)
        => Math.Ceiling(x);

    public static double Floor(double x)
        => Math.Floor(x);

    public static double Round(double x, int digits)
        => Math.Round(x, digits);

    public static dynamic Min(params dynamic[] numbers)
        => numbers.Min() ?? throw new InvalidOperationException("Invalid types for function Min()");

    public static dynamic Max(params dynamic[] numbers)
        => numbers.Max() ?? throw new InvalidOperationException("Invalid types for function Max()");

    public static dynamic Vector(params dynamic[] values)
    {
        float x = 0, y = 0, z = 0, w = 0;

        if (values.Length == 0)
            throw new ArgumentException("Vector must have at least one element.", nameof(values));

        if (values.Length == 1 && values[0] is Complex complex)
        {
        }
        else if (values.Length == 2
                && TypeFactory.DynamicConvert<float>(values[0], out x)
                && TypeFactory.DynamicConvert<float>(values[1], out y))
        {
            return new Vector2(x, y);
        }
        else if (values.Length == 3
                && TypeFactory.DynamicConvert<float>(values[0], out x)
                && TypeFactory.DynamicConvert<float>(values[1], out y)
                && TypeFactory.DynamicConvert<float>(values[1], out z))
        {
            return new Vector3(x, y, z);
        }
        else if (values.Length == 4
                && TypeFactory.DynamicConvert<float>(values[0], out x)
                && TypeFactory.DynamicConvert<float>(values[1], out y)
                && TypeFactory.DynamicConvert<float>(values[1], out z)
                && TypeFactory.DynamicConvert<float>(values[2], out w))
        {
            return new Vector4(x, y, z, w);
        }

        var strValues = string.Join(", ", values);
        throw new InvalidCastException($"Don't know how to cast {strValues} to vector");
    }
}
