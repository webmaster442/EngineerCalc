namespace DynamicEvaluator.Expressions.Specific;

internal static class Extensions
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
}
