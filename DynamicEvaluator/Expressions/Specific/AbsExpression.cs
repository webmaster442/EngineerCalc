namespace DynamicEvaluator.Expressions.Specific;

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
            ? $"{{ abs({Child}) }}"
            : $"abs({Child})";
    }
}
