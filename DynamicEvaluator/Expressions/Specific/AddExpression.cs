
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

        var leftVar = newLeft as VariableExpression;
        var rightVar = newRight as VariableExpression;
        var leftMultiply = newLeft as MultiplyExpression;
        var rightMultiply = newRight as MultiplyExpression;

        if (leftVar != null)
        {
            // x + x  =>  2 * x
            if (leftVar.Identifier == rightVar?.Identifier)
            {
                return new MultiplyExpression(leftVar, new ConstantExpression(2));
            }
            // x + (2 * x)  =>  3 * x
            if (rightMultiply?.Right is VariableExpression rightMulVar
                && rightMultiply.Left is ConstantExpression rightMulConst
                && leftVar.Identifier == rightMulVar.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(leftVar, rightMulConst.Value + 1);
            }
            // x + (x * 2)  =>  3 * x
            if (rightMultiply?.Left is VariableExpression rightMulVar2
                && rightMultiply.Right is ConstantExpression rightMulConst2
                && leftVar.Identifier == rightMulVar2.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(leftVar, rightMulConst2.Value + 1);
            }
        }

        if (rightVar != null)
        {
            // (2 * x) + x  =>  3 * x
            if (leftMultiply?.Right is VariableExpression leftMulVar
                && leftMultiply.Left is ConstantExpression leftMulConst
                && rightVar.Identifier == leftMulVar.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(rightVar, leftMulConst.Value + 1);
            }
            // (x * 2) + x  =>  3 * x
            if (leftMultiply?.Left is VariableExpression leftMulVar2
                && leftMultiply.Right is ConstantExpression leftMulConst2
                && rightVar.Identifier == leftMulVar2.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(rightVar, leftMulConst2.Value + 1);
            }
        }

        // x + y;  no simplification
        return new AddExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 + value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex ?
            $"{{ {Left.ToLatex()} + {Right.ToLatex()} }}" 
            : $"({Left} + {Right})";
    }
}
