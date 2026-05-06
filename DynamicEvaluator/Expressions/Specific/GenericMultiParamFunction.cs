//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class GenericMultiParamFunction : GenericFunctionExpression
{
    private readonly Func<Result[], Result> _func;

    public GenericMultiParamFunction(Func<Result[], Result> func, string name, IExpression[] parameters)
        : base(name, parameters)
    {
        _func = func;
    }

    public override Result Evaluate(VariablesAndConstantsCollection variables)
    {
        Result[] args = new Result[_parameters.Length];
        for (int i=0; i<_parameters.Length; i++)
        {
            args[i] = _parameters[i].Evaluate(variables);
        }
        return _func.Invoke(args);
    }

    public override IExpression Simplify()
        => new GenericMultiParamFunction(_func, _name, _parameters);
}
