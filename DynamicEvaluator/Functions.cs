namespace DynamicEvaluator;

public static class Functions
{
    public static dynamic Ln(dynamic a)
    {
        if (a is double d)
            return Math.Log(d);

        throw new InvalidOperationException($"Can't perform Ln function on type {a.GetType()}");
    }

    public static dynamic Pow(dynamic value1, dynamic value2)
    {
        if (value1 is double v1 || value2 is double v2)
        {
            return Math.Pow(value1, value2);
        }

        throw new InvalidOperationException($"Can't perform Pow function on type {value1.GetType()} and {value2.GetType()}");
    }
}
