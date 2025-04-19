using System.Globalization;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class ConstantExpression : IExpression
{
    public ConstantExpression(dynamic value)
    {
        Value = value;
    }

    public dynamic Value { get; }

    public IExpression Differentiate(string byVariable)
        => new ConstantExpression(0L);

    public dynamic Evaluate(VariablesAndConstantsCollection variables) 
        => Value;

    public IExpression Simplify()
        => new ConstantExpression(Value);

    public string ToLatex()
        => $"{{ {Value.ToString(CultureInfo.InvariantCulture)} }}";

    public override string ToString()
        => Value.ToString(CultureInfo.InvariantCulture);
}
