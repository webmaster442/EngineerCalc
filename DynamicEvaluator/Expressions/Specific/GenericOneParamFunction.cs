//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class GenericOneParamFunction : GenericFunctionExpression
{
    private readonly Func<Result, Result> _func;

    public GenericOneParamFunction(Func<Result, Result> func, string name, IExpression[] parameters) 
        : base(name, parameters)
    {
        _func = func;
    }

    public override Result Evaluate(VariablesAndConstantsCollection variables)
        => _func.Invoke(_parameters[0].Evaluate(variables));

    public override IExpression Simplify()
        => new GenericOneParamFunction(_func, _name, _parameters);
}
