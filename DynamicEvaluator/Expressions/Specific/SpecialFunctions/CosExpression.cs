//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class CosExpression : UnaryExpression
{
    public CosExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new MultiplyExpression(new NegateExpression(new SinExpression(Child)), Child.Differentiate(byVariable));
    }

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
            return new ConstantExpression(-1);
        }
        return new CosExpression(newChild);
    }

    protected override Result Evaluate(Result value)
        => TypeFunctions.Cos(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ cos({Child.ToLatex()}) }}"
            : $"cos({Child})";
    }
}
