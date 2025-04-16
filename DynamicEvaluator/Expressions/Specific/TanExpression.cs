namespace DynamicEvaluator.Expressions.Specific;

internal sealed class TanExpression : UnaryExpression
{
    public TanExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new MultiplyExpression(new ExponentExpression(new CosExpression(Child), new ConstantExpression(-2)), Child.Differentiate(byVariable));
    }

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        return new TanExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => Functions.Tan(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ tan({Child}) }}"
            : $"tan({Child})";
    }
}
