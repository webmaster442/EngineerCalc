
namespace DynamicEvaluator.Expressions.Specific;

internal sealed class EqualExpression : BinaryExpression
{
    public EqualExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Can't differentiate an expression with the < operator");

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        var leftConst = newLeft as ConstantExpression;
        var rightConst = newRight as ConstantExpression;

        if (leftConst != null && rightConst != null)
        {
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }

        return new EqualExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 == value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left} == {Right} }}"
            : $"({Left} == {Right})";
    }
}
