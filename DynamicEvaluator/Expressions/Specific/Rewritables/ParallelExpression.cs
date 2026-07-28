//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class ParallelExpression : RewritableExpression
{
    public ParallelExpression(IExpression first, IExpression second)
    {
        _rewritten = new DivideExpression(new MultiplyExpression(first, second), new AddExpression(first, second));
    }
}
