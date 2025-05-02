using DynamicEvaluator.Expressions.Specific.SpecialFunctions;

namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class ArcCtgExpression : RewritableFunctionExpression
{
    public ArcCtgExpression(IExpression original) : base(original)
    {
    }

    protected override IExpression RewriteTo(IExpression original)
    {
        return new ArcTanExpression(new DivideExpression(new ConstantExpression(1L), original));
    }
}