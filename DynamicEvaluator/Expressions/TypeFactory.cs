using System.Globalization;

namespace DynamicEvaluator.Expressions;

internal sealed class TypeFactory
{
    internal static dynamic CreateType(string value, object? data)
    {
        if (data is not Type type)
            throw new InvalidCastException($"Don't know how to parse {value}");

        if (type == typeof(string))
            return value;

        if (type == typeof(double))
            return double.Parse(value, CultureInfo.InvariantCulture);

        if (type == typeof(long))
            return long.Parse(value, CultureInfo.InvariantCulture);

        if (type == typeof(bool))
            return bool.Parse(value);

        throw new InvalidCastException($"Don't know how to parse {value}");
    }
}
