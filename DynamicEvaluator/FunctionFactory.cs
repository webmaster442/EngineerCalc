using System.Collections;

using DynamicEvaluator.Expressions.Specific;
using DynamicEvaluator.Expressions.Specific.Rewritables;
using DynamicEvaluator.Expressions.Specific.SpecialFunctions;
using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator;

internal sealed class FunctionFactory : IEnumerable<string>
{
    private readonly Dictionary<string, Func<Result, Result>> _oneParamFunctions;
    private readonly Dictionary<string, Func<Result, Result, Result>> _twoParamFunctions;
    private readonly Dictionary<string, Func<Result[], Result>> _multiParamFunctions;
    private readonly Dictionary<string, int> _rewriteFunctions;

    public FunctionFactory()
    {
        _rewriteFunctions = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "ctg", 1 },
            { "arcctg", 1 },
            { "deg", 1 },
            { "grad", 1 },
            { "degtorad", 1 },
            { "gradtorad", 1 },
        };
        _oneParamFunctions = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { nameof(TypeFunctions.Abs), TypeFunctions.Abs },
            { nameof(TypeFunctions.Ln), TypeFunctions.Ln },
            { nameof(TypeFunctions.Sin), TypeFunctions.Sin },
            { nameof(TypeFunctions.ArcSin ), TypeFunctions.ArcSin },
            { nameof(TypeFunctions.Cos), TypeFunctions.Cos },
            { nameof(TypeFunctions.ArcCos), TypeFunctions.ArcCos },
            { nameof(TypeFunctions.Tan), TypeFunctions.Tan },
            { nameof(TypeFunctions.ArcTan), TypeFunctions.ArcTan },
            { nameof(TypeFunctions.FromHex), TypeFunctions.FromHex },
            { nameof(TypeFunctions.FromBin ), TypeFunctions.FromBin },
            { nameof(TypeFunctions.FromHexSigned), TypeFunctions.FromHexSigned },
            { nameof(TypeFunctions.FromBinSigned ), TypeFunctions.FromBinSigned },
            { nameof(TypeFunctions.ToHex), TypeFunctions.ToHex },
            { nameof(TypeFunctions.ToBin), TypeFunctions.ToBin },
            { nameof(TypeFunctions.Sqrt), TypeFunctions.Sqrt },
        };
        _twoParamFunctions = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { nameof(TypeFunctions.Cplx), TypeFunctions.Cplx },
            { nameof(TypeFunctions.CplxPlr), TypeFunctions.CplxPlr },
            { nameof(TypeFunctions.Log), TypeFunctions.Log },
            { nameof(TypeFunctions.Pow), TypeFunctions.Pow },
            { nameof(TypeFunctions.Root), TypeFunctions.Root },
            { nameof(TypeFunctions.Gcd), TypeFunctions.Gcd },
            { nameof(TypeFunctions.Lcm), TypeFunctions.Lcm },
        };
        _multiParamFunctions = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { nameof(TypeFunctions.Max), TypeFunctions.Max },
            { nameof(TypeFunctions.Min), TypeFunctions.Min },
            { nameof(TypeFunctions.Sum), TypeFunctions.Sum },
            { nameof(TypeFunctions.Average), TypeFunctions.Average },
            { nameof(TypeFunctions.Random), TypeFunctions.Random },
            { nameof(TypeFunctions.Array), TypeFunctions.Array },
            { nameof(TypeFunctions.Count), TypeFunctions.Count },
        };
    }

    public bool IsFunction(string name)
    {
        return _rewriteFunctions.ContainsKey(name)
            || _oneParamFunctions.ContainsKey(name)
            || _twoParamFunctions.ContainsKey(name)
            || _multiParamFunctions.ContainsKey(name);
    }

    public IExpression Create(string name, IExpression[] parameters)
    {
        if (!ValidateParameterCount(name, parameters.Length))
            throw new InvalidOperationException($"No overload of {name} found that takes {parameters.Length} parameters");

        return name switch
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
            _ => CreateGeneric(name, parameters),
        };
    }

    private IExpression CreateGeneric(string name, IExpression[] parameters)
    {
        if (_multiParamFunctions.TryGetValue(name, out Func<Result[], Result>? multiFunction))
        {
            return new GenericMultiParamFunction(multiFunction, name, parameters);
        }

        if (_twoParamFunctions.TryGetValue(name, out Func<Result, Result, Result>? twoParamFunction))
        {
            return parameters.Length != 2
                ? throw new InvalidOperationException($"Function {name} requires exactly 2 parameters")
                : (IExpression)new GenericTwoParamFunction(twoParamFunction, name, parameters);
        }

        if (_oneParamFunctions.TryGetValue(name, out Func<Result, Result>? oneParamFunction))
        {
            return parameters.Length != 1
                ? throw new InvalidOperationException($"Function {name} requires exactly 1 parameter")
                : (IExpression)new GenericOneParamFunction(oneParamFunction, name, parameters);
        }

        throw new InvalidOperationException($"Unknown function: {name}");
    }

    private bool ValidateParameterCount(string name, int count)
    {
        if (_rewriteFunctions.TryGetValue(name, out int expectedCount))
        {
            return count == expectedCount;
        }
        if ((_oneParamFunctions.ContainsKey(name) && count == 1)
            || (_twoParamFunctions.ContainsKey(name) && count == 2)
            || _multiParamFunctions.ContainsKey(name))
        {
            return true;
        }

        return false;
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _oneParamFunctions.Keys
            .Concat(_rewriteFunctions.Keys)
            .Concat(_twoParamFunctions.Keys)
            .Concat(_multiParamFunctions.Keys)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
