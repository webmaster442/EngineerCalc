//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


namespace DynamicEvaluator.Expressions.Specific;

internal static class SimplifyHelpers
{
    public static bool IsIntegerMultupleOfPi(this IExpression expression)
    {
        if (expression is VariableExpression variable
            && variable.Identifier == "pi")
        {
            return true;
        }

        if (expression is MultiplyExpression multiplyExpression)
        {
            if (multiplyExpression.Left is ConstantExpression leftConst
                && leftConst.Value is long
                && multiplyExpression.Right is VariableExpression rightVar
                && rightVar.Identifier == "pi")
            {
                return true;
            }
            if (multiplyExpression.Right is ConstantExpression rightConst
                && rightConst.Value is long
                && multiplyExpression.Left is VariableExpression leftVar
                && leftVar.Identifier == "pi")
            {
                return true;
            }
        }
        return false;
    }

    internal static IExpression MakeVariableMultplyConstant(VariableExpression variable, dynamic value)
    {
        if (value == 1)
        {
            return variable;
        }
        else if (value == 0)
        {
            return new ConstantExpression(0L);
        }
        return new MultiplyExpression(variable, new ConstantExpression(value));
    }

    internal static IExpression MakeExponentMultiplyConstant(VariableExpression variable, dynamic value)
    {
        if (value == 1)
        {
            return variable;
        }
        else if (value == 0)
        {
            return new ConstantExpression(1L);
        }
        return new ExponentExpression(variable, new ConstantExpression(value));
    }

}
