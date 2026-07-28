//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class GradExpression : RewritableExpression
{
    public GradExpression(IExpression original)
    {
        _rewritten = new DivideExpression(new MultiplyExpression(original, new ConstantExpression(200L)), new VariableExpression("pi"));
    }
}
