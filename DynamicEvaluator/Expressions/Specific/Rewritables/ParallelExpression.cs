//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class ParallelExpression : RewritableTwoparamFunctionExpression
{
    public ParallelExpression(IExpression first, IExpression second) : base(first, second)
    {
    }

    protected override IExpression RewriteTo(IExpression first, IExpression second)
    {
        return new DivideExpression(new MultiplyExpression(first, second), new AddExpression(first, second));
    }
}
