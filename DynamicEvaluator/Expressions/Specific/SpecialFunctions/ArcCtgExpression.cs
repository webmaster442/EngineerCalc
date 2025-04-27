namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class ArcCtgExpression : UnaryExpression
{
    public ArcCtgExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new DivideExpression(new ConstantExpression(-1L),
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
        return new ArcCtgExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => Functions.ArcCtg(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ arcctg({Child}) }}"
            : $"arcctg({Child})";
    }
}
