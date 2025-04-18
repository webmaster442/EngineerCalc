namespace DynamicEvaluator.Expressions.Specific;

internal sealed class CtgExpression : UnaryExpression
{
    public CtgExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new NegateExpression(new MultiplyExpression(new ExponentExpression(new SinExpression(Child), new ConstantExpression(-2)), Child.Differentiate(byVariable)));
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
            return new ConstantExpression(double.PositiveInfinity);
        }
        return new CtgExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => Functions.Ctg(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ ctg({Child}) }}"
            : $"ctg({Child})";
    }
}
