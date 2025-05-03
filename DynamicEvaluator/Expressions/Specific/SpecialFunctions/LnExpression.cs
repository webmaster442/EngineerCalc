namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class LnExpression : UnaryExpression
{
    public LnExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new DivideExpression(new ConstantExpression(1L), Child);
    }

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(Evaluate(childConst.Value));
        }
        else if (newChild is VariableExpression variable && variable.Identifier == "e")
        {
            return new ConstantExpression(1L);
        }
        return new LnExpression(newChild);
    }

    protected override dynamic Evaluate(dynamic value)
        => Functions.Ln(value);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ ln({Child.ToLatex()}) }}"
            : $"ln({Child})";
    }
}
