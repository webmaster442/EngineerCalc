namespace DynamicEvaluator.Expressions.Specific.Rewritables;

internal sealed class NandExpression : RewritableExpression
{
    public NandExpression(IExpression left, IExpression right)
    {
        _rewritten = new LogicNegateExpression(new AndExpression(left, right));
    }
}
