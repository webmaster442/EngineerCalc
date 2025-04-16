using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace DynamicEvaluator.Types;

internal sealed class TypeFactory
{
    internal static dynamic CreateFraction(dynamic value1, dynamic value2)
    {
        long l1 = (long)value1;
        long l2 = (long)value2;
        return new Fraction(l1, l2);
    }

    public static bool DynamicConvert<TTarget>(dynamic source, [NotNullWhen(true)] out TTarget? result)
    {
        if (source is TTarget directMatch)
        {
            result = directMatch;
            return true;
        }

        try
        {
            Type sourceType = source.GetType();
            Type targetType = typeof(TTarget);

            var implicitOperator = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Concat(targetType.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .FirstOrDefault(m =>
                    m.Name == "op_Implicit" &&
                    m.ReturnType == targetType &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType.IsAssignableFrom(sourceType));

            if (implicitOperator != null)
            {
                object value = implicitOperator.Invoke(null, [source])!;
                result = (TTarget)value;
                return true;
            }

            if (source is IConvertible && typeof(TTarget).GetInterfaces().Contains(typeof(IConvertible)))
            {
                result = (TTarget)Convert.ChangeType(source, typeof(TTarget));
                return true;
            }

            result = default;
            return false;
        }
        catch (Exception)
        {
            result = default;
            return false;
        }
    }

    internal static dynamic CreateType(string value, object? data)
    {
        if (data is not Type type)
            throw new InvalidCastException($"Don't know how to parse {value}");

        if (type == typeof(string))
            return value;

        if (type == typeof(double))
        {
            string cleaned = value.Replace("_", "");
            return double.Parse(cleaned, CultureInfo.InvariantCulture);
        }

        if (type == typeof(long))
        {
            string cleaned = value.Replace("_", "");
            return long.Parse(cleaned, CultureInfo.InvariantCulture);
        }

        if (type == typeof(bool))
            return bool.Parse(value);

        throw new InvalidCastException($"Don't know how to parse {value}");
    }
}
