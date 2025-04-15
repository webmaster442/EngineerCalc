namespace DynamicEvaluator.Expressions.Specific;

internal sealed class ConstantExpression : IExpression
{
    public ConstantExpression(dynamic value)
    {
        Value = value;
    }

    public dynamic Value { get; }

    public IExpression Differentiate(string byVariable)
        => new ConstantExpression(0);

    public dynamic Evaluate(IReadOnlyDictionary<string, dynamic> variables) 
        => Value;

    public IExpression Simplify()
        => new ConstantExpression(Value);

    public string ToLatex()
        => $"{{ {Value} }}";

    public override string ToString()
        => Value.ToString();
}
