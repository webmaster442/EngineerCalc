using System.Numerics;

namespace DynamicEvaluator;

public static class Functions
{
    public static dynamic Ln(dynamic value)
    {
        if (value is double d)
            return Math.Log(d);

        if (value is long l)
            return Math.Log(l);

        if (value is Complex c)
            return Complex.Log(c);

        throw new InvalidOperationException($"Can't perform Ln function on type {value.GetType()}");
    }

    public static dynamic Pow(dynamic value1, dynamic value2)
    {
        if (value1 is double || value2 is double)
        {
            return Math.Pow(value1, value2);
        }

        if (value1 is Complex || value2 is Complex)
        {
            return Complex.Pow(value1, value2);
        }

        throw new InvalidOperationException($"Can't perform Pow function on type {value1.GetType()} and {value2.GetType()}");
    }

    public static dynamic Sin(dynamic value)
    {
        if (value is double d)
            return Math.Sin(d);

        if (value is Complex c)
            return Complex.Sin(c);

        throw new InvalidOperationException($"Can't perform Sin function on type {value.GetType()}");
    }

    public static dynamic Cos(dynamic value)
    {
        if (value is double d)
            return Math.Cos(d);

        if (value is Complex c)
            return Complex.Sin(c);

        throw new InvalidOperationException($"Can't perform Sin function on type {value.GetType()}");
    }
}
