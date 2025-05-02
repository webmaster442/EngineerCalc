namespace DynamicEvaluator.Expressions.Specific.SpecialFunctions;

internal sealed class LogExpression : BinaryExpression
{
    public LogExpression(IExpression left, IExpression right) : base(left, right)
    {
    }

    public override IExpression Differentiate(string byVariable)
    {
        return new MultiplyExpression(new DivideExpression(new ConstantExpression(1L), new LnExpression(Right)),
                                      new DivideExpression(new ConstantExpression(1L), Left));
    }

    public override IExpression Simplify()
    {
        var newLeft = Left.Simplify();
        var newRight = Right.Simplify();

        if (newLeft is ConstantExpression leftConst
            && newRight is ConstantExpression rightConst)
        {
            // two constants
            return new ConstantExpression(Evaluate(leftConst.Value, rightConst.Value));
        }
        else
        {
            return new LogExpression(newLeft, newRight);
        }
    }

    protected override dynamic Evaluate(dynamic value1, dynamic value2)
        => Functions.Log(value1, value2);

    protected override string Render(bool emitLatex)
    {
        return emitLatex
            ? $"{{ log_{Left} {Right} }}"
            : $"log({Left}, {Right})";
    }
}
