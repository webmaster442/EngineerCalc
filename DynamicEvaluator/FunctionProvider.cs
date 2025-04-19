using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Json;

using DynamicEvaluator.Expressions.Specific;

namespace DynamicEvaluator;

internal sealed class FunctionProvider
{
    private sealed record class FunctionEntry(MethodInfo Method, int Parameters);

    private readonly Dictionary<string, List<FunctionEntry>> _functions;
    private readonly List<string> _documentations;

    public FunctionProvider()
    {
        _functions = new Dictionary<string, List<FunctionEntry>>(StringComparer.OrdinalIgnoreCase);
        _documentations = new List<string>();
    }

    public void FillFrom(Type type)
    {
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static).OrderBy(x => x.Name);
        var docs = typeof(Functions).Assembly.GetManifestResourceNames();

        foreach (var method in methods)
        {
            if (!_functions.TryGetValue(method.Name, out List<FunctionEntry>? value))
            {
                value = new List<FunctionEntry>();
                _functions.Add(method.Name, value);
            }

            value.Add(new FunctionEntry(Method: method, Parameters: method.GetParameters().Length));
        }

        _documentations.AddRange(docs);
    }

    public IEnumerable<string> GetFunctionNames()
        => _functions.Keys;

    public bool IsFunction(string name)
        => _functions.ContainsKey(name);

    public string GetDocumentation(string function)
    {
        var selector = $".{function}.md";
        var fullName = _documentations.Where(x => x.EndsWith(function, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

        if (string.IsNullOrEmpty(fullName))
            return string.Empty;

        using var manifestStream = typeof(Functions).Assembly.GetManifestResourceStream(fullName);
        if (manifestStream != null)
        {
            using var reader = new StreamReader(manifestStream);
            return reader.ReadToEnd();
        }

        return string.Empty;
    }

    public IEnumerable<int> GetParameterCounts(string name)
    {
        if (!_functions.ContainsKey(name))
            return Enumerable.Empty<int>();

        return _functions[name].Select(x => x.Parameters);
    }

    public IExpression Create(string function, IReadOnlyList<IExpression> parameters)
    {
        IEnumerable<int> parameterCounts = GetParameterCounts(function);
        if (!parameterCounts.Any())
            throw new InvalidOperationException($"Unknown function: {function}");

        ValidateParameterCount(function, parameters.Count, parameterCounts);

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

    private void ValidateParameterCount(string function, int passedParameterCount, IEnumerable<int> overloadParameterCounts)
    {
        bool check = overloadParameterCounts
            .Where(x => x == passedParameterCount)
        .Any();

        if (!check)
            throw new InvalidOperationException($"No overload of {function} found that takes {passedParameterCount} parameters");
    }

    private LambdaExpression CreateLambda(string function, IReadOnlyList<IExpression> parameters)
    {
        var entry = _functions[function]
            .Where(x => x.Parameters == parameters.Count)
            .First();

        return new LambdaExpression(entry.Method, parameters);
    }

}
