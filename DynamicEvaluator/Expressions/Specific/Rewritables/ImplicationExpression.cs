namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class ImplicationExpression : RewritableExpression
{
    public ImplicationExpression(IExpression left, IExpression right)
    {
        _rewritten = new OrExpression(
            new LogicNegateExpression(left),
            right);
    }
}
