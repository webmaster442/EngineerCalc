namespace DynamicEvaluator.Types;

public static class TypeHelper
{
    public static bool IsLong(dynamic d)
    {
        return d is long;
    }

    public static bool IsIntegralType(dynamic value)
    {
        return value is sbyte
            or byte
            or short
            or ushort
            or int
            or uint
            or long
            or ulong
            or char;
    }
}
