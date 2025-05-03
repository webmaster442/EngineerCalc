namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class GradToRadExpression : RewritableFunctionExpression
{
    public GradToRadExpression(IExpression original) : base(original)
    {
    }

    protected override IExpression RewriteTo(IExpression original)
    {
        return new MultiplyExpression(new DivideExpression(original, new ConstantExpression(200L)), new VariableExpression("pi"));
    }
}