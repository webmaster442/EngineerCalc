namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class EquivalenceExpression : RewritableExpression
{
    public EquivalenceExpression(IExpression left, IExpression right)
    {
        _rewritten = new OrExpression(
            new AndExpression(left, right),
            new AndExpression(new LogicNegateExpression(left), new LogicNegateExpression(right)));
    }
}
