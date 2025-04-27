namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class ArcSinExpression : UnaryExpression
{
    public ArcSinExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new DivideExpression(new ConstantExpression(1L),
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

    protected override dynamic Evaluate(dynamic value)
        => Functions.ArcSin(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ arcsin({Child}) }}"
            : $"arcsin({Child})";
    }
}
