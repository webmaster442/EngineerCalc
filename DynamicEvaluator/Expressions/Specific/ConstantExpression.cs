//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

    public bool Equals(IExpression? other)
    {
        return other is ConstantExpression otherConst
            && Value == otherConst.Value;
    }

    public dynamic Evaluate(VariablesAndConstantsCollection variables)
        => Value;

    public IExpression Simplify()
        => new ConstantExpression(Value);

    public string ToLatex()
        => $"{{ {Value.ToString(CultureInfo.InvariantCulture)} }}";

    public override string ToString()
        => Value.ToString(CultureInfo.InvariantCulture);
}
