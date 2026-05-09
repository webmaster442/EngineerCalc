//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using DynamicEvaluator.Expressions.Specific.SpecialFunctions;

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class CosecExpression : RewritableFunctionExpression
{
    public CosecExpression(IExpression original) : base(original)
    {
    }
    protected override IExpression RewriteTo(IExpression original)
    {
        return new DivideExpression(new ConstantExpression(1L), new SinExpression(original));
    }
}
