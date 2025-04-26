namespace DynamicEvaluator.Expressions.Specific;

internal sealed class ModuloExpression : BinaryExpression
{
    public ModuloExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException("Can't differentiate an expression with the % operator");

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        if (newLeft is ConstantExpression leftConst)
        {
            //0 % x => 0
            if (leftConst.Value == 0)
            {
                return new ConstantExpression(0L);
            }
            if (newRight is ConstantExpression rightConst)
            {
                //1 % 0 => exception
                if (rightConst.Value == 0L)
                {
                    throw new DivideByZeroException();
                }

                //two constants
                return new ConstantExpression(leftConst.Value % rightConst.Value);
            }
        }
        else if (newRight is ConstantExpression constant &&
                 constant.Value == 1)
        {
            // x % 1 = 0
            return new ConstantExpression(0L);
        }
        else if (newLeft is VariableExpression leftVar
                 && newRight is VariableExpression rightVar
                 && leftVar.Identifier == rightVar.Identifier)
        {
            return new ConstantExpression(0L);
        }
        
        return new ModuloExpression(newLeft, newRight);
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => value1 % value2;

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ {Left} mod {Right} }}"
            : $"({Left} % {Right})";
    }
}
