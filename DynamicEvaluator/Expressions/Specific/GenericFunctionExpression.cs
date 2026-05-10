//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific;

internal abstract class GenericFunctionExpression : IExpression
{
    protected readonly string _name;
    protected readonly IExpression[] _parameters;

    public GenericFunctionExpression(string name, IExpression[] parameters)
    {
        _name = name;
        _parameters = parameters;
    }

    public IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException($"Function {_name} can't be differentiated");

    public bool Equals(IExpression? other)
    {
        return other != null
            && other is GenericFunctionExpression generic
            && generic._name == _name
            && generic._parameters.Length == _parameters.Length
            && generic._parameters.SequenceEqual(_parameters);
    }

    public abstract Result Evaluate(VariablesAndConstantsCollection variables);

    public abstract IExpression Simplify();

    public string ToLatex()
    {
        var subs = string.Join(',', _parameters.Select(x => x.ToLatex()));
        return $"{{ {_name}({subs}) }}";
    }

    public override string ToString()
    {
        var subs = string.Join(", ", _parameters.Select(x => x.ToString()));
        return $"{_name}({subs})";
    }
}
