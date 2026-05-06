//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class ArcSinExpression : UnaryExpression
{
    public ArcSinExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new DivideExpression(Child.Differentiate(byVariable),
            new RootExpression(
                new SubtractExpression(new ConstantExpression(1L), new ExponentExpression(Child, new ConstantExpression(2L))),
                new ConstantExpression(2L)));
    }

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        return new ArcSinExpression(newChild);
    }

    protected override Result Evaluate(Result value)
        => TypeFunctions.ArcSin(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ sin^{{-1}}({Child.ToLatex()}) }}"
            : $"arcsin({Child})";
    }
}
