namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class ArcCosExpression : UnaryExpression
{
    public ArcCosExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new NegateExpression(new DivideExpression(Child.Differentiate(byVariable),
            new RootExpression(
                new SubtractExpression(new ConstantExpression(1L), new ExponentExpression(Child, new ConstantExpression(2L))),
                new ConstantExpression(2L))));
    }

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        return new ArcCosExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => Functions.ArcCos(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ cos^{{-1}}({Child.ToLatex()}) }}"
            : $"arccos({Child})";
    }
}
