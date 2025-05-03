namespace DynamicEvaluator.Expressions.Specific;

internal sealed class SubtractExpression : BinaryExpression
{
    public SubtractExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => new SubtractExpression(Left.Differentiate(byVariable), Right.Differentiate(byVariable));

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        var leftConst = newLeft as ConstantExpression;
        var rightConst = newRight as ConstantExpression;
        var rightNegate = newRight as NegateExpression;

        if (leftConst != null && rightConst != null)
        {
            // two constants
            return new ConstantExpression(leftConst.Value - rightConst.Value);
        }
        if (leftConst?.Value == 0)
        {
            // 0 - y
            if (rightNegate != null)
            {
                // y = -u (--u)
                return rightNegate.Child;
            }
            return new NegateExpression(newRight);
        }
        if (rightConst?.Value == 0)
        {
            // x - 0
            return newLeft;
        }
        if (rightNegate != null)
        {
            // x - -y
            return new AddExpression(newLeft, rightNegate.Child);
        }
        // x - y;  no simplification
        return new SubtractExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 - value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex ?
            $"{{ {Left.ToLatex()} - {Right.ToLatex()} }}"
            : $"({Left} - {Right})";
    }
}
