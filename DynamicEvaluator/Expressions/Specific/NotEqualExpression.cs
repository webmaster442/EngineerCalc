//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class NotEqualExpression : BinaryExpression
{
    public NotEqualExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Can't differentiate an expression with the < operator");

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        if (newLeft is ConstantExpression leftConst
            && newRight is ConstantExpression rightConst)
        {
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }

        return new NotEqualExpression(newLeft, newRight);
    }

    protected override Result Evaluate(Result value1, Result value2)
        => Result.FromBoolean(value1 != value2);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left.ToLatex()} \\neq {Right.ToLatex()} }}"
            : $"({Left} != {Right})";
    }
}
