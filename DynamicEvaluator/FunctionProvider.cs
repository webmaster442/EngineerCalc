using System.Reflection;

using DynamicEvaluator.Expressions.Specific;
using DynamicEvaluator.Expressions.Specific.Rewritables;
using DynamicEvaluator.Expressions.Specific.SpecialFunctions;

namespace DynamicEvaluator;

internal sealed class FunctionProvider
{
    private sealed record class FunctionEntry
    {
        public required MethodInfo Method { get; init; }
        public required int ParameterCount { get; init; }
        public required bool IsParamsMethod { get; init; }
    }

    private readonly Dictionary<string, List<FunctionEntry>> _functions;
    private readonly List<string> _documentations;
    private readonly string[] _rewriteFunctions;

    public FunctionProvider()
    {
        _functions = new Dictionary<string, List<FunctionEntry>>(StringComparer.OrdinalIgnoreCase);
        _documentations = new List<string>();
        _rewriteFunctions = ["ctg", "arcctg", "deg", "grad", "degtorad", "gradtorad"];
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

            (int count, bool isParams) = GetParameterCount(method.GetParameters());

            value.Add(new FunctionEntry
            {
                Method = method,
                ParameterCount = count,
                IsParamsMethod = isParams,
            });
        }

        _documentations.AddRange(docs);
    }

    private static (int count, bool isParams) GetParameterCount(ParameterInfo[] parameterInfos)
    {
        int count = parameterInfos.Length;
        bool isParams = parameterInfos.Any(pi => pi.GetCustomAttribute<ParamArrayAttribute>() != null);
        return (count, isParams);
    }

    public IEnumerable<string> GetFunctionNames()
        => _functions.Keys.Concat(_rewriteFunctions);

    public bool IsFunction(string name)
        => _functions.ContainsKey(name) || _rewriteFunctions.Contains(name, StringComparer.OrdinalIgnoreCase);

    public IExpression Create(string function, IReadOnlyList<IExpression> parameters)
    {
        if (!_rewriteFunctions.Contains(function))
        {
            if (!_functions.TryGetValue(function, out var overloads))
                throw new InvalidOperationException($"Unknown function: {function}");

            if (!ValidateParameterCount(overloads, parameters.Count))
                throw new InvalidOperationException($"No overload of {function} found that takes {parameters.Count} parameters");
        }

        //Special functions
        return function switch
        {
            "abs" => new AbsExpression(parameters[0]),
            "sin" => new SinExpression(parameters[0]),
            "arcsin" => new ArcSinExpression(parameters[0]),
            "cos" => new CosExpression(parameters[0]),
            "arccos" => new ArcCosExpression(parameters[0]),
            "tan" => new TanExpression(parameters[0]),
            "arctan" => new ArcTanExpression(parameters[0]),
            "ctg" => new CtgExpression(parameters[0]),
            "arcctg" => new ArcCtgExpression(parameters[0]),
            "ln" => new LnExpression(parameters[0]),
            "log" => new LogExpression(parameters[0], parameters[1]),
            "root" => new RootExpression(parameters[0], parameters[1]),
            "deg" => new DegExpression(parameters[0]),
            "grad" => new GradExpression(parameters[0]),
            "degtorad" => new DegToRadExpression(parameters[0]),
            "gradtorad" => new GradToRadExpression(parameters[0]),
            _ => CreateLambda(function, parameters),
        };
    }

    private static bool ValidateParameterCount(List<FunctionEntry> overloads, int count)
    {
        if (overloads.Any(func => func.IsParamsMethod))
        {
            return true;
        }
        bool check = overloads.Any(func => func.ParameterCount == count);
        return check;
    }

    private GenericFunctionExpression CreateLambda(string function, IReadOnlyList<IExpression> parameters)
    {
        FunctionEntry? byCount = _functions[function].FirstOrDefault(overload => overload.ParameterCount == parameters.Count);
        if (byCount != null)
        {
            return new GenericFunctionExpression(byCount.Method, byCount.IsParamsMethod, parameters);
        }

        var byIsParams = _functions[function].FirstOrDefault(overload => overload.IsParamsMethod);
        if (byIsParams == null)
            throw new InvalidOperationException($"No overload of {function} is found that takes {parameters.Count} parameters");

        return new GenericFunctionExpression(byIsParams.Method, byIsParams.IsParamsMethod, parameters);
        
    }

}
