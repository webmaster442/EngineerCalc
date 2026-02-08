//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class SubtractExpression : BinaryExpression
{
    public SubtractExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => new SubtractExpression(Left.Differentiate(byVariable), Right.Differentiate(byVariable));

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        var leftConst = newLeft as ConstantExpression;
        var rightConst = newRight as ConstantExpression;
        var rightNegate = newRight as NegateExpression;

        if (leftConst != null && rightConst != null)
        {
            // two constants
            return new ConstantExpression(leftConst.Value - rightConst.Value);
        }
        if (leftConst?.Value == 0)
        {
            // 0 - y
            if (rightNegate != null)
            {
                // y = -u (--u)
                return rightNegate.Child;
            }
            return new NegateExpression(newRight);
        }
        if (rightConst?.Value == 0)
        {
            // x - 0
            return newLeft;
        }
        if (rightNegate != null)
        {
            // x - -y
            return new AddExpression(newLeft, rightNegate.Child);
        }

        var leftVar = newLeft as VariableExpression;
        var rightVar = newRight as VariableExpression;
        var leftMultiply = newLeft as MultiplyExpression;
        var rightMultiply = newRight as MultiplyExpression;

        if (leftVar != null)
        {
            // x - x  =>  0
            if (leftVar.Identifier == rightVar?.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(leftVar, 0L);
            }
            // x - (2 * x)  =>  -1 * x
            if (rightMultiply?.Right is VariableExpression rightMulVar
                && rightMultiply.Left is ConstantExpression rightMulConst
                && leftVar.Identifier == rightMulVar.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(leftVar, 1L - rightMulConst.Value);
            }
            // x - (x * 2)  =>  -1 * x
            if (rightMultiply?.Left is VariableExpression rightMulVar2
                && rightMultiply.Right is ConstantExpression rightMulConst2
                && leftVar.Identifier == rightMulVar2.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(leftVar, 1L - rightMulConst2.Value);
            }
        }

        if (rightVar != null)
        {
            // (2 * x) - x  =>  1 * x
            if (leftMultiply?.Right is VariableExpression leftMulVar
                && leftMultiply.Left is ConstantExpression leftMulConst
                && rightVar.Identifier == leftMulVar.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(rightVar, leftMulConst.Value - 1L);
            }
            // (x * 2) - x  =>  1 * x
            if (leftMultiply?.Left is VariableExpression leftMulVar2
                && leftMultiply.Right is ConstantExpression leftMulConst2
                && rightVar.Identifier == leftMulVar2.Identifier)
            {
                return SimplifyHelpers.MakeVariableMultplyConstant(rightVar, leftMulConst2.Value - 1L);
            }
        }

        // x - y;  no simplification
        return new SubtractExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 - value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex ?
            $"{{ {Left.ToLatex()} - {Right.ToLatex()} }}"
            : $"({Left} - {Right})";
    }
}
