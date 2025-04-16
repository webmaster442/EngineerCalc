using System.Reflection;

using DynamicEvaluator.Expressions.Specific;

namespace DynamicEvaluator;

internal sealed class FunctionProvider
{
    private readonly Dictionary<string, (MethodInfo method, int paramCount)> _table;

    public FunctionProvider()
    {
        _table = new Dictionary<string, (MethodInfo method, int paramCount)>(StringComparer.OrdinalIgnoreCase);
    }

    public void FillFrom(Type type)
    {
        var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        foreach (var method in methods)
        {
            _table.Add(method.Name, (method, method.GetParameters().Length));
        }
    }

    public int GetParameterCount(string name)
    {
        if (!_table.ContainsKey(name))
            return -1;

        return _table[name].paramCount;
    }

    public IExpression Create(string function, IReadOnlyList<IExpression> parameters)
    {
        int paramCount = GetParameterCount(function);
        if (paramCount < 1)
            throw new InvalidOperationException($"Unknown function: {function}");

        HasCount(parameters, paramCount);

        //Special functions
        return function switch
        {
            "cos" => new CosExpression(parameters[0]),
            "sin" => new SinExpression(parameters[0]),
            "tan" => new TanExpression(parameters[0]),
            "ctg" => new CtgExpression(parameters[0]),
            "ln" => new LnExpression(parameters[0]),
            "log" => new LogExpression(parameters[0], parameters[1]),
            "root" => new RootExpression(parameters[0], parameters[1]),
            _ => CreateLambda(function, parameters),
        };
    }

    private LambdaExpression CreateLambda(string function, IReadOnlyList<IExpression> parameters)
    {
        var (method, paramCount) = _table[function];
        return new LambdaExpression(method, parameters);
    }

    private static void HasCount<T>(IReadOnlyList<T> list, int count)
    {
        if (list.Count != count)
            throw new InvalidOperationException($"Expected {count} arguments, got {list.Count}");
    }
}
