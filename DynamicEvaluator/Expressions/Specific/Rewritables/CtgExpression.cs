using DynamicEvaluator.Expressions.Specific.SpecialFunctions;

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class CtgExpression : RewritableFunctionExpression
{
    public CtgExpression(IExpression original) : base(original)
    {
    }

    protected override IExpression RewriteTo(IExpression original)
    {
        return new DivideExpression(new ConstantExpression(1L), new TanExpression(original));
    }
}
