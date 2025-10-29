namespace DynamicEvaluator.Expressions.Specific;

internal sealed class MultiplyExpression : BinaryExpression
{
    public MultiplyExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return
            new AddExpression(new MultiplyExpression(Left, Right.Differentiate(byVariable)),
                              new MultiplyExpression(Left.Differentiate(byVariable), Right));
    }

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        var leftConst = newLeft as ConstantExpression;
        var rightConst = newRight as ConstantExpression;
        var leftNegate = newLeft as NegateExpression;
        var rightNegate = newRight as NegateExpression;

        if (leftConst != null && rightConst != null)
        {
            // two constants
            return new ConstantExpression(leftConst.Value * rightConst.Value);
        }
        if (leftConst != null)
        {
            if (leftConst.Value == 0)
            {
                // 0 * y
                return new ConstantExpression(0L);
            }
            if (leftConst.Value == 1)
            {
                // 1 * y
                return newRight;
            }
            if (leftConst.Value == -1)
            {
                // -1 * y
                if (rightNegate != null)
                {
                    // y = -u (-y = --u)
                    return rightNegate.Child;
                }
                return new NegateExpression(newRight);
            }
        }
        else if (rightConst != null)
        {
            if (rightConst.Value == 0)
            {
                // x * 0
                return new ConstantExpression(0L);
            }
            if (rightConst.Value == 1)
            {
                // x * 1
                return newLeft;
            }
            if (rightConst.Value == -1)
            {
                // x * -1
                if (leftNegate != null)
                {
                    // x = -u (-x = --u)
                    return leftNegate.Child;
                }
                return new NegateExpression(newLeft);
            }
        }
        else if (leftNegate != null && rightNegate != null)
        {
            // -x * -y
            return new MultiplyExpression(leftNegate.Child, rightNegate.Child);
        }

        var leftVar = newLeft as VariableExpression;
        var rightVar = newRight as VariableExpression;
        var leftExponent = newLeft as ExponentExpression;
        var rightExponent = newRight as ExponentExpression;

        // x * x = x^2
        if (leftVar != null && rightVar != null && leftVar.Equals(rightVar))
        {
            return new ExponentExpression(newLeft, new ConstantExpression(2L));
        }

        // x*x^2
        if (rightExponent?.Left is VariableExpression rightExpVar
            && rightExponent.Right is ConstantExpression rightExpConst
            && leftVar?.Identifier == rightExpVar.Identifier)
        {
            return SimplifyHelpers.MakeExponentMultiplyConstant(leftVar, rightExpConst.Value + 1L);
        }
        // x^2*x
        if (leftExponent?.Left is VariableExpression leftExpVar
            && leftExponent.Right is ConstantExpression leftExpConst
            && rightVar?.Identifier == leftExpVar.Identifier)
        {
            return SimplifyHelpers.MakeExponentMultiplyConstant(rightVar, leftExpConst.Value + 1L);
        }

        // x * y
        return new MultiplyExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 * value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left.ToLatex()} \\cdot {Right.ToLatex()} }}"
            : $"({Left} * {Right})";
    }
}
