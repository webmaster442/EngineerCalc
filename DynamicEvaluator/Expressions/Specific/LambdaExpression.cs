using System.Reflection;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class LambdaExpression : IExpression
{
    private readonly MethodInfo _method;
    private readonly IReadOnlyList<IExpression> _parameters;
    private readonly int _parameterCount;

    public LambdaExpression(MethodInfo method, IReadOnlyList<IExpression> parameters)
    {
        _method = method;
        _parameters = parameters;
        _parameterCount = _method.GetParameters().Length;
    }

    public IExpression Differentiate(string byVariable)
        => throw new InvalidOperationException($"Expression with {_method.Name} can't be differentiated");

    public dynamic Evaluate(VariablesAndConstantsCollection variables)
    {
        dynamic[] args = new dynamic[_parameterCount];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = _parameters[i].Evaluate(variables);
        }
        return _method.Invoke(null, args) ?? throw new InvalidOperationException($"Method invoke failed: {_method.Name}");
    }

    public IExpression Simplify()
        => new LambdaExpression(_method, _parameters);

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
