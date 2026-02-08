//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class NegateExpression : UnaryExpression
{
    public NegateExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => new NegateExpression(Child.Differentiate(byVariable));

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(-childConst.Value);
        }
        return new NegateExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => -value;

    protected override string Render(bool emitLatex)
    {
        return emitLatex ?
            $"- {{ {Child.ToLatex()} }}"
            : $"(-{Child})";
    }
}
