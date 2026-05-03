using System.Numerics;

namespace DynamicEvaluator.TypeSystem.Internals;

internal static class IntegerMath
{
    public static BigInteger Lcm(BigInteger a, BigInteger b)
    {
        checked
        {
            return (a * b) / BigInteger.GreatestCommonDivisor(a, b);
        }
    }
}
