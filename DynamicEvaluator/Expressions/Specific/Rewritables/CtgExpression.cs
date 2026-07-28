//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.Expressions.Specific.SpecialFunctions;

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class CtgExpression : RewritableExpression
{
    public CtgExpression(IExpression original)
    {
        _rewritten = new DivideExpression(new ConstantExpression(1L), new TanExpression(original));
    }
}
