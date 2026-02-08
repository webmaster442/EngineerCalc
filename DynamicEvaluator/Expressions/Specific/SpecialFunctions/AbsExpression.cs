//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal class AbsExpression : UnaryExpression
{
    public AbsExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new DivideExpression(Child.Differentiate(byVariable), new AbsExpression(Child));
    }

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        return new AbsExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => Functions.Abs(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ \\mid {Child.ToLatex()} \\mid }}"
            : $"abs({Child})";
    }
}
