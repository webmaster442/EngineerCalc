namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class SinExpression : UnaryExpression
{
    public SinExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new MultiplyExpression(new CosExpression(Child), Child.Differentiate(byVariable));
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
            return new ConstantExpression(0L);
        }
        return new SinExpression(newChild);
    }



    protected override dynamic Evaluate(dynamic value)
        => Functions.Sin(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ sin({Child.ToLatex()}) }}"
            : $"sin({Child})";
    }
}
