//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.TypeSystem;

namespace DynamicEvaluator.Expressions.Specific;

internal sealed class GenericParameterlessFunction : GenericFunctionExpression
{
    private readonly Func<Result> _func;

    public GenericParameterlessFunction(Func<Result> parameterlessFunction, string name)
        : base(name, Array.Empty<IExpression>())
    {
        _func = parameterlessFunction;
    }

    public override Result Evaluate(VariablesAndConstantsCollection variables)
        => _func.Invoke();

    public override IExpression Simplify()
        => new GenericParameterlessFunction(_func, _name);
}
