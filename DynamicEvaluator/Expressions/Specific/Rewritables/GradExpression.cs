namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class GradExpression : RewritableFunctionExpression
{
    public GradExpression(IExpression original) : base(original)
    {
    }

    protected override IExpression RewriteTo(IExpression original)
    {
        return new DivideExpression(new MultiplyExpression(original, new ConstantExpression(200L)), new VariableExpression("pi"));
    }
}
