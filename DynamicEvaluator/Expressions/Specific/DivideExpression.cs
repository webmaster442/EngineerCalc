using DynamicEvaluator.Types;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class DivideExpression : BinaryExpression
{
    public DivideExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return
            new DivideExpression(new SubtractExpression(new MultiplyExpression(Left.Differentiate(byVariable), Right),
                                 new MultiplyExpression(Left, Right.Differentiate(byVariable))),
                                 new ExponentExpression(Right, new ConstantExpression(2)));
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
            if (rightConst.Value == 0L)
            {
                throw new DivideByZeroException();
            }
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }
        if (leftConst != null && leftConst.Value == 0L)
        {
            // 0 / y
            if (rightConst?.Value == 0L)
            {
                throw new DivideByZeroException();
            }
            return new ConstantExpression(0L);
        }
        else if (rightConst != null)
        {
            if (rightConst.Value == 0)
            {
                // x / 0
                throw new DivideByZeroException();
            }
            if (rightConst.Value == 1)
            {
                // x / 1
                return newLeft;
            }
            if (rightConst.Value == -1)
            {
                // x / -1
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
            // -x / -y
            return new DivideExpression(leftNegate.Child, rightNegate.Child);
        }

        var leftVar = newLeft as VariableExpression;
        var rightVar = newRight as VariableExpression;
        var leftExponent = newLeft as ExponentExpression;
        var rightExponent = newRight as ExponentExpression;

        if (leftVar != null
            && rightVar != null
            && leftVar.Identifier == rightVar.Identifier)
        {
            // x / x
            return new ConstantExpression(1L);
        }

        // x/x^2
        if (rightExponent?.Left is VariableExpression rightExpVar
            && rightExponent.Right is ConstantExpression rightExpConst
            && leftVar?.Identifier == rightExpVar.Identifier)
        {
            return SimplifyHelpers.MakeExponentMultiplyConstant(leftVar, 1L - rightExpConst.Value);
        }
        // x^2/x
        if (leftExponent?.Left is VariableExpression leftExpVar
            && leftExponent.Right is ConstantExpression leftExpConst
            && rightVar?.Identifier == leftExpVar.Identifier)
        {
            return SimplifyHelpers.MakeExponentMultiplyConstant(rightVar, leftExpConst.Value - 1L);
        }

        // x / y;  no simplification
        return new DivideExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
    {
        if (TypeHelper.IsLong(value1) && TypeHelper.IsLong(value2))
        {
            return TypeFactory.CreateFraction(value1, value2);
        }
        return value1 / value2;
    }

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left.ToLatex()} \\over {Right.ToLatex()} }}"
            : $"({Left} / {Right})";
    }
}
