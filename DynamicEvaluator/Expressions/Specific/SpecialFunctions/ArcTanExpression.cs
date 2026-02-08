//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class ArcTanExpression : UnaryExpression
{
    public ArcTanExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new DivideExpression(Child.Differentiate(byVariable),
               new AddExpression(new ExponentExpression(Child, new ConstantExpression(2L)), new ConstantExpression(1L)));
    }

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        return new ArcTanExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => Functions.ArcTan(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ tan^{{-1}}({Child.ToLatex()}) }}"
            : $"arctan({Child})";
    }
}
