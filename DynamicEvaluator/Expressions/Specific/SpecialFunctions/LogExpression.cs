//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class LogExpression : BinaryExpression
{
    public LogExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new MultiplyExpression(new DivideExpression(new ConstantExpression(1L), new LnExpression(Right)),
                                      new DivideExpression(new ConstantExpression(1L), Left));
    }

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        if (newLeft is ConstantExpression leftConst
            && newRight is ConstantExpression rightConst)
        {
            // two constants
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }
        else
        {
            return new LogExpression(newLeft, newRight);
        }
    }

    protected override Result Evaluate(Result value1, Result value2)
        => TypeFunctions.Log(value1, value2);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ log_{Left.ToLatex()} {Right.ToLatex()} }}"
            : $"log({Left}, {Right})";
    }
}
