using DynamicEvaluator.Expressions.Specific;

namespace DynamicEvaluator;

internal class FunctionTable
{
    private readonly Dictionary<string, int> _table;

    public FunctionTable()
    {
        _table = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
    }

    public void FillFrom(Type type)
    {
        var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        foreach (var method in methods)
        {
            _table.Add(method.Name, method.GetParameters().Length);
        }
    }

    public int GetParameterCount(string name)
    {
        if (!_table.ContainsKey(name))
            return -1;

        return _table[name];
    }

    public IExpression Create(string function, IReadOnlyList<IExpression> parameters)
    {
        int paramCount = GetParameterCount(function);
        if (paramCount < 1)
            throw new InvalidOperationException($"Unknown function: {function}");

        HasCount(parameters, paramCount);

        return function switch
        {
            "cos" => new CosExpression(parameters[0]),
            "sin" => new SinExpression(parameters[0]),
            "tan" => new TanExpression(parameters[0]),
            "ctg" => new CtgExpression(parameters[0]),
            "ln" => new LnExpression(parameters[0]),
            "log" => new LogExpression(parameters[0], parameters[1]),
            _ => throw new InvalidOperationException("Function not implemented")
        };
    }

    private static void HasCount<T>(IReadOnlyList<T> list, int count)
    {
        if (list.Count != count)
            throw new InvalidOperationException($"Expected {count} arguments, got {list.Count}");
    }
}
