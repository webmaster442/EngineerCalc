using System.Resources;

using DynamicEvaluator.Expressions;
using DynamicEvaluator.Expressions.Specific;
using DynamicEvaluator.Logic;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DynamicEvaluator;

public static class IExpressionExtensions
{
    private static IEnumerable<IExpression> Flatten(this IExpression expression)
    {
        Stack<IExpression> expressions = new Stack<IExpression>();

        expressions.Push(expression);

        while (expressions.Count > 0)
        {
            var n = expressions.Pop();

            if (n != null)
            {
                yield return n;
            }

            if (n is BinaryExpression bin)
            {
                if (bin.Left != null)
                    expressions.Push(bin.Left);
                if (bin.Right != null)
                    expressions.Push(bin.Right);
            }
            else if (n is UnaryExpression un)
            {
                if (un.Child != null)
                    expressions.Push(un.Child);
            }
        }
    }

    public static List<int> GetMinterms(this IExpression expression, out string[] variableNames)
    {
        var flatExpression = expression.Flatten();

        variableNames = flatExpression
            .OfType<VariableExpression>()
            .Select(v => v.Identifier)
            .Distinct()
            .ToArray();


        if (variableNames.Length > 24)
            throw new InvalidOperationException("Too many variables");

        int combinations = 1 << variableNames.Length;
        List<int> result = new List<int>();
        VariablesAndConstantsCollection variables = new();

        for (int i = 0; i < combinations; i++)
        {
            string pattern = Utilities.GetBinaryValue(i, variableNames.Length);
            for (int j = 0; j < variableNames.Length; j++)
            {
                variables[variableNames[j]] = pattern[j] == '1' ? true : false;
            }
            if (expression.Evaluate(variables) is bool b && b == true)
            {
                result.Add(i);
            }
        }
        return result;

    }

    private static bool IsLogicExpressionNode(IExpression expression)
    {
        return expression is AndExpression
            || expression is OrExpression
            || expression is LogicNegateExpression
            || (expression is ConstantExpression constant && constant.Value is bool)
            || expression is VariableExpression;
    }

    public static bool IsLogicExpression(this IExpression expression)
    {
        return expression.Flatten().All(IsLogicExpressionNode);
    }

    public static bool TrySimplfyAsLogicExpression(this IExpression expression, out IExpression? simplified)
    {
        if (!IsLogicExpression(expression))
        {
            simplified = expression;
            return false;
        }

        try
        {
            var minterms = GetMinterms(expression, out string[] names);
            var expressionString = QuineMcclusky.GetSimplified(minterms, Enumerable.Empty<int>(), names.Length, new QuineMcCluskeyConfig
            {
                VariableNamesToUse = names,
            });

            ExpressionFactory factory = new ExpressionFactory();
            simplified = factory.Create(expressionString);
            return true;
        }
        catch (Exception)
        {
            simplified = null;
            return false;
        }
    }

}
