using System.Collections;

using DynamicEvaluator.Expressions.Specific;
using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator;

internal sealed class FunctionFactory : IEnumerable<string>
{
    private readonly Dictionary<string, Func<Result, Result>> _oneParamFunctions;
    private readonly Dictionary<string, Func<Result, Result, Result>> _twoParamFunctions;
    private readonly Dictionary<string, Func<Result[], Result>> _multiParamFunctions;

    public FunctionFactory()
    {
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
        };
    }

    public bool IsFunction(string name)
    {
        return _oneParamFunctions.ContainsKey(name)
            || _twoParamFunctions.ContainsKey(name)
            || _multiParamFunctions.ContainsKey(name);
    }

    public IExpression Create(string name, IExpression[] parameters)
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

    public IEnumerator<string> GetEnumerator()
    {
        return _oneParamFunctions.Keys
            .Concat(_twoParamFunctions.Keys)
            .Concat(_multiParamFunctions.Keys)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
