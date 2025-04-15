
namespace DynamicEvaluator.Expressions.Specific;

internal sealed class AddExpression : BinaryExpression
{
    public AddExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => new AddExpression(Left.Differentiate(byVariable), Right.Differentiate(byVariable));

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        var leftConst = newLeft as ConstantExpression;
        var rightConst = newRight as ConstantExpression;

        if (leftConst != null && rightConst != null)
        {
            // two constants
            return new ConstantExpression(leftConst.Value + rightConst.Value);
        }
        if (leftConst?.Value == 0)
        {
            // 0 + y
            return newRight;
        }
        if (rightConst?.Value == 0)
        {
            // x + 0
            return newLeft;
        }
        if (newRight is NegateExpression rightNegate)
        {
            // x + -y;  return x - y;  (this covers -x + -y case too)
            return new SubtractExpression(newLeft, rightNegate.Child);
        }
        if (newLeft is NegateExpression leftNegate)
        {
            // -x + y
            return new SubtractExpression(newRight, leftNegate.Child);
        }
        // x + y;  no simplification
        return new AddExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 + value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex ?
            $"{{ {Left} + {Right} }}" 
            : $"({Left} + {Right})";
    }
}
