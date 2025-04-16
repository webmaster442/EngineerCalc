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
            if (rightConst.Value == 0)
            {
                throw new DivideByZeroException();
            }
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }
        if (leftConst?.Value == 0)
        {
            // 0 / y
            if (rightConst?.Value == 0)
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
            ? $"{{ {Left} \\over {Right} }}"
            : $"({Left} / {Right})";
    }
}
