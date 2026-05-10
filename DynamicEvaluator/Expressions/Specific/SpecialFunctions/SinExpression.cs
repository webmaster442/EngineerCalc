//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class SinExpression : UnaryExpression
{
    public SinExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => new MultiplyExpression(new CosExpression(Child), Child.Differentiate(byVariable));

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        if (newChild.IsIntegerMultupleOfPi())
        {
            return new ConstantExpression(0L);
        }
        return new SinExpression(newChild);
    }

    protected override Result Evaluate(Result value)
        => TypeFunctions.Sin(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ sin({Child.ToLatex()}) }}"
            : $"sin({Child})";
    }
}
