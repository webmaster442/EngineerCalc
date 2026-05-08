//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using DynamicEvaluator.TypeSystem;

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
                && leftConst.Value.TypeState == TypeState.Integer
                && multiplyExpression.Right is VariableExpression rightVar
                && rightVar.Identifier == "pi")
            {
                return true;
            }
            if (multiplyExpression.Right is ConstantExpression rightConst
                && rightConst.Value.TypeState == TypeState.Integer
                && multiplyExpression.Left is VariableExpression leftVar
                && leftVar.Identifier == "pi")
            {
                return true;
            }
        }
        return false;
    }

    internal static IExpression MakeVariableMultplyConstant(VariableExpression variable, Result value)
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

    internal static IExpression MakeExponentMultiplyConstant(VariableExpression variable, Result value)
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
