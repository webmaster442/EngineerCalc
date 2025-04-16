using System.Numerics;

using DynamicEvaluator.Types;

namespace DynamicEvaluator;

public static class Functions
{
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

    public static dynamic Cos(dynamic value)
    {
        if (value is Complex c)
            return Complex.Cos(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Cos(d);

        throw new InvalidOperationException($"Can't perform Cos function on type {value.GetType()}");
    }

    public static dynamic Tan(dynamic value)
    {
        if (value is Complex c)
            return Complex.Tan(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return Math.Tan(d);

        throw new InvalidOperationException($"Can't perform Tan function on type {value.GetType()}");
    }

    public static dynamic Ctg(dynamic value)
    {
        if (value is Complex c)
            return 1.0 / Complex.Tan(c);

        if (TypeFactory.DynamicConvert<double>(value, out double d))
            return 1.0 / Math.Tan(d);

        throw new InvalidOperationException($"Can't perform Ctg function on type {value.GetType()}");
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

    public static long FromHex(string str)
        => long.Parse(str, System.Globalization.NumberStyles.HexNumber);

    public static long FromBin(string str)
        => long.Parse(str, System.Globalization.NumberStyles.BinaryNumber);
}
