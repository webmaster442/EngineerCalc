namespace DynamicEvaluator.Types;

internal static class TypeHelper
{
    public static bool IsLong(dynamic d)
    {
        return d is long;
    }
}
