namespace DynamicEvaluator.Expressions.Specific;

internal sealed class ExponentExpression : BinaryExpression
{
    public ExponentExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        if (Right is ConstantExpression)
        {
            // f(x) = g(x)^n
            // f'(x) = n * g'(x) * g(x)^(n-1)
            return
                new MultiplyExpression(new MultiplyExpression(Right, Left.Differentiate(byVariable)),
                                       new ExponentExpression(Left, new SubtractExpression(Right, new ConstantExpression(1L))));
        }
        var simple = Left?.Simplify();
        if (simple is ConstantExpression constant)
        {
            // f(x) = a^g(x)
            // f'(x) = (ln a) * g'(x) * a^g(x)
            var a = constant.Value;
            return new MultiplyExpression(new MultiplyExpression(new ConstantExpression(Functions.Ln(a)), Right.Differentiate(byVariable)), new ExponentExpression(simple, Right));
        }

        throw new InvalidOperationException("Expression can't be differentiated due to exponent");
    }

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        var leftConst = newLeft as ConstantExpression;
        var rightConst = newRight as ConstantExpression;

        if (leftConst != null && rightConst != null)
        {
            // two constants
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }
        if (rightConst != null)
        {
            if (rightConst.Value == 0)
            {
                // x ^ 0
                return new ConstantExpression(1L);
            }
            if (rightConst.Value == 1)
            {
                // x ^ 1
                return newLeft;
            }
        }
        else if (leftConst?.Value == 0)
        {
            // 0 ^ y
            return new ConstantExpression(0L);
        }
        // x ^ y;  no simplification
        return new ExponentExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => Functions.Pow(value1, value2);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left.ToLatex()} ^ {Right.ToLatex()} }}"
            : $"({Left} ^ {Right})";
    }
}
