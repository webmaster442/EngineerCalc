//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class RootExpression : BinaryExpression
{
    public RootExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        if (Right is ConstantExpression)
        {
            // f(x) = g(x)^n
            // f'(x) = n * g'(x) * g(x)^(n-1)

            var newRight = new DivideExpression(new ConstantExpression(1L), Right);

            return
                new MultiplyExpression(new MultiplyExpression(newRight, Left.Differentiate(byVariable)),
                                       new ExponentExpression(Left, new SubtractExpression(newRight, new ConstantExpression(1))));
        }
        throw new InvalidOperationException("Can't differentiate function");
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
        return new ExponentExpression(newLeft, new DivideExpression(new ConstantExpression(1), newRight));
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => Functions.Root(value1, value2);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ \\sqrt[{Right.ToLatex()}] {{{Left.ToLatex()}}} }}"
            : $"root({Left}, {Right})";
    }
}
