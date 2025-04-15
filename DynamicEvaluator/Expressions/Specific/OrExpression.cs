
namespace DynamicEvaluator.Expressions.Specific;

internal sealed class OrExpression : BinaryExpression
{
    public OrExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Can't differentiate an expression with the | operator");

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        var leftConst = newLeft as ConstantExpression;
        var rightConst = newRight as ConstantExpression;

        if (leftConst != null && rightConst != null)
        {
            // two constants
            return new ConstantExpression(leftConst.Value | rightConst.Value);
        }
        if (leftConst != null && newRight is VariableExpression rightVariable)
        {
            if (leftConst.Value == true)
                return new VariableExpression(rightVariable.Identifier);
            if (leftConst.Value == false)
                return new ConstantExpression(false);
        }
        if (rightConst != null && newLeft is VariableExpression leftVariable)
        {
            if (rightConst.Value == true)
                return new ConstantExpression(true);
            if (rightConst.Value == false)
                return new VariableExpression(leftVariable.Identifier);
        }
        return new AndExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 | value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left} \\lor {Right} }}"
            : $"({Left} & {Right})";
    }
}
