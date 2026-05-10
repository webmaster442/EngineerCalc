//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class CoshExpression : UnaryExpression
{
    public CoshExpression(IExpression child) : base(child)
    {
    }
    public override IExpression Differentiate(string byVariable)
        => new MultiplyExpression(new SinhExpression(Child), Child.Differentiate(byVariable));

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        return new CoshExpression(newChild);
    }

    protected override Result Evaluate(Result value)
        => TypeFunctions.Cosh(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ cosh({Child.ToLatex()}) }}"
            : $"cosh({Child})";
    }
}
