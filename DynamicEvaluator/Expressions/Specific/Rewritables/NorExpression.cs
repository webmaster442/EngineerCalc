namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class NorExpression : RewritableExpression
{
    public NorExpression(IExpression left, IExpression right)
    {
        _rewritten = new LogicNegateExpression(new OrExpression(left, right));
    }
}
