namespace DynamicEvaluator.Expressions.Specific;

internal sealed class LogicNegateExpression : UnaryExpression
{
    public LogicNegateExpression(IExpression child) : base(child)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Can't differentiate an expression with the ~ operator");

    public override IExpression Simplify()
    {
        var newChild = Child.Simplify();
        if (newChild is ConstantExpression childConst)
        {
            // child is constant
            return new ConstantExpression(~childConst.Value);
        }
        return new LogicNegateExpression(newChild);
    }

    protected override dynamic Evalulate(dynamic value)
        => ~value;

    protected override string Render(bool emitLatex)
    {
        return emitLatex
             ? $"{{ \\neg {Child} }}"
             : $"(~{Child})";
    }
}
