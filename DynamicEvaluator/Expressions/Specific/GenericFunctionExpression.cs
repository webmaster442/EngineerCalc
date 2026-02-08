using System.ComponentModel;
using System.Reflection;

using DynamicEvaluator.Types;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class GenericFunctionExpression : IExpression
{
    private readonly MethodInfo _method;
    private readonly bool _isParams;
    private readonly IReadOnlyList<IExpression> _parameters;

    public GenericFunctionExpression(MethodInfo method, bool isParams, IReadOnlyList<IExpression> parameters)
    {
        _isParams = isParams;
        _method = method;
        _parameters = parameters;
    }

    public IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException($"Expression with {_method.Name} can't be differentiated");

    public bool Equals(IExpression? other)
    {
        return other is GenericFunctionExpression otherFunc
            && _method == otherFunc._method
            && _isParams == otherFunc._isParams
            && _parameters.SequenceEqual(otherFunc._parameters);
    }

    public dynamic Evaluate(VariablesAndConstantsCollection variables)
    {
        dynamic[] args = new dynamic[_parameters.Count];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = _parameters[i].Evaluate(variables);
        }

        if (_isParams)
        {
            args = new dynamic[] { args };
        }

        return _method.Invoke(null, args) ?? throw new InvalidOperationException($"Method invoke failed: {_method.Name}");
    }


    public IExpression Simplify()
        => new GenericFunctionExpression(_method, _isParams, _parameters);

    public string ToLatex()
    {
        var subs = string.Join(',', _parameters.Select(x => x.ToLatex()));
        return $"{{ {_method.Name}({subs}) }}";
    }

    public override string ToString()
    {
        var subs = string.Join(',', _parameters.Select(x => x.ToString()));
        return $"{_method.Name}({subs})";
    }
}
