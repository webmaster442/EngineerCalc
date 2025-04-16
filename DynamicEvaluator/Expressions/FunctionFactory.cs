using DynamicEvaluator.Expressions.Specific;

namespace DynamicEvaluator.Expressions;

internal static class FunctionFactory
{
    public static IExpression Create(string function, IReadOnlyList<IExpression> parameters)
    {
        switch (function)
        {
            case "cos":
                parameters.HasCount(1);
                return new CosExpression(parameters[0]);
            case "sin":
                parameters.HasCount(1);
                return new SinExpression(parameters[0]);
            default:
                throw new InvalidOperationException($"Unknown function: {function}");
        }
    }

    private static void HasCount<T>(this IReadOnlyList<T> list, int count)
    {
        if (list.Count != count)
            throw new InvalidOperationException($"Expected {count} arguments, got {list.Count}");
    }
}
