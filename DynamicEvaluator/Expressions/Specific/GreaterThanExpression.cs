
namespace DynamicEvaluator.Expressions.Specific;

internal sealed class GreaterThanExpression : BinaryExpression
{
    public GreaterThanExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Can't differentiate an expression with the < operator");

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        if (newLeft is ConstantExpression leftConst
            && newRight is ConstantExpression rightConst)
        {
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }

        return new GreaterThanExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 > value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left.ToLatex()} > {Right.ToLatex()} }}"
            : $"({Left} > {Right})";
    }
}
